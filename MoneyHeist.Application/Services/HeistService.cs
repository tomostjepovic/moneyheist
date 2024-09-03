using Microsoft.EntityFrameworkCore;
using MoneyHeist.Application.Interfaces;
using MoneyHeist.Data.Dtos.Heist;
using MoneyHeist.Data.Entities;
using MoneyHeist.Data.Models;
using MoneyHeist.DataAccess;

namespace MoneyHeist.Application.Services
{
    public class HeistService : IHeistService
    {
        private readonly RepoContext repoContext;
        private readonly ISkillService skillService;

        public HeistService(RepoContext _repoContext, ISkillService _skillService)
        {
            repoContext = _repoContext;
            skillService = _skillService;
        }

        public async Task<ServiceResult> UpdateHeistSkills(int heistID, HeistSkillsDto updateHeistSkillsDto)
        {
            var errors = new List<string>();
            var skillsErrors = ValidateHeistSkills(updateHeistSkillsDto.Skills);
            if (skillsErrors != null)
            {
                errors.AddRange(skillsErrors);
            }

            var heist = repoContext.Heists.SingleOrDefault(x => x.ID == heistID);

            if (heist == null)
            {
                // TODO: raise exception?
                errors.Add("Heist nof found");
            } else
            {
                if (HeistHasStarted(heist))
                {
                    errors.Add("Heist already started");
                }
            }

            if (errors.Any())
            {
                return ServiceResult.ErrorResult("Validation errors", errors);
            }

            DeleteHeistSkils(heistID);

            foreach (var skillDto in updateHeistSkillsDto.Skills)
            {
                await InsertHeistToSkill(heistID, skillDto);
            }

            return ServiceResult.SuccessResult();
        }

        public bool HeistHasStarted(Heist heist)
        {
            return heist.Start <= DateTime.Now;
        }

        private void DeleteHeistSkils(int heistID)
        {
            var existingSkills = repoContext.HeistToSkills.Where(x => x.HeistID == heistID);

            repoContext.HeistToSkills.RemoveRange(existingSkills);
        } 

        public async Task<HeistDto> GetHeistById(int id)
        {
            var heist = repoContext.Heists.SingleOrDefault(x => x.ID == id);
            if (heist == null)
            {
                return null;
            }
            return HeistToHeistDto(heist);
        }

        public async Task<CreateHeistServiceResult> CreateHeist(HeistDto heistDto)
        {
            var errors = await ValidateCreateHeistAsync(heistDto);

            if (errors.Any())
            {
                return new CreateHeistServiceResult(false, "Validation errors", errors);
            }

            var heist = new Heist();
            heist.Name = heistDto.Name;
            heist.Start = heistDto.StartTime.Value;
            heist.End = heistDto.EndTime.Value;
            heist.Location = heistDto.Location;

            await repoContext.AddAsync(heist);
            await repoContext.SaveChangesAsync();

            heistDto.ID = heist.ID;

            foreach (var heistToSkillDto in heistDto.Skills)
            {
                await InsertHeistToSkill(heist.ID, heistToSkillDto);
            }            

            return new CreateHeistServiceResult(true)
            {
                HeistID = heist.ID
            };
        }

        private async Task<List<string>> ValidateCreateHeistAsync(HeistDto heistDto)
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrWhiteSpace(heistDto.Name))
            {
                errors.Add("Name is required");
            }

            if (string.IsNullOrWhiteSpace(heistDto.Location))
            {
                errors.Add("Location is required");
            }

            if (!heistDto.StartTime.HasValue)
            {
                errors.Add("Start time is required");
            }

            if (!heistDto.EndTime.HasValue)
            {
                errors.Add("End time is required");
            } else
            {
                if (heistDto.EndTime.Value <= DateTime.Now)
                {
                    errors.Add("End time must be in future");
                }
            }

            if (heistDto.StartTime.HasValue && heistDto.EndTime.HasValue)
            {
                if (heistDto.StartTime.Value >= heistDto.EndTime.Value)
                {
                    errors.Add("End time must be greater than start time");
                }
            }

            if (await HeistWithSameNameExistsAsync(heistDto.Name))
            {
                errors.Add("Heist with same name exists");
            }

            var skillsErrors = ValidateHeistSkills(heistDto.Skills);
            if (skillsErrors != null)
            {
                errors.AddRange(skillsErrors);
            }            

            return errors;
        }

        private async Task<bool> HeistWithSameNameExistsAsync(string name)
        {
            return await repoContext.Heists.AnyAsync(x => x.Name == name);
        }

        private List<string> ValidateHeistSkills(List<HeistToSkillDto>? heistSkills)
        {
            List<string> errors = new List<string>();

            if (heistSkills == null || !heistSkills.Any())
            {
                errors.Add("At least one skill is required");
                return errors;
            }

            var hasDuplicateSkils = HeistSkillsHasDuplicates(heistSkills);
            if (hasDuplicateSkils)
            {
                errors.Add($"Member cannot have duplicate skills");
            }

            var skillWithIncorectLevelExists = HeistSkillsHasInvalidLevel(heistSkills);
            if (skillWithIncorectLevelExists)
            {
                errors.Add($"Skill level is incorrect.");
            }

            return errors;
        }

        private bool HeistSkillsHasDuplicates(List<HeistToSkillDto> heistSkills)
        {
            return heistSkills.GroupBy(x => new { x.Name, x.Level }).Any(grp => grp.Count() > 1);
        }

        public bool HeistSkillsHasInvalidLevel(List<HeistToSkillDto> memberSkills)
        {
            return memberSkills.Any(x => !x.LevelIsValid);
        }

        private async Task InsertHeistToSkill(int heistId, HeistToSkillDto heistToSkillDto)
        {
            var skill = await skillService.GetOrInsertSkillIfNotExists(heistToSkillDto.Name);
            var heistToSkill = new HeistToSkill
            {
                HeistID = heistId,
                SkillID = skill.ID,
                Level = heistToSkillDto.LevelNumeric,
                Members = heistToSkillDto.Members
            };

            await repoContext.AddAsync(heistToSkill);
            await repoContext.SaveChangesAsync();
        }   
        

        // mappers
        private HeistDto HeistToHeistDto(Heist heist)
        {
            return new HeistDto
            {
                ID = heist.ID,
                EndTime = heist.End,
                StartTime = heist.Start,
                Location = heist.Location,
                Name = heist.Name
            };
        }
    }
}
