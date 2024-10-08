﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MoneyHeist.Application.Interfaces;
using MoneyHeist.Application.Mappers;
using MoneyHeist.Data.AppSettings;
using MoneyHeist.Data.Dtos.Heist;
using MoneyHeist.Data.Dtos.Member;
using MoneyHeist.Data.Entities;
using MoneyHeist.Data.ErrorCodes;
using MoneyHeist.Data.Models;
using MoneyHeist.DataAccess;

namespace MoneyHeist.Application.Services
{
    public class HeistService : IHeistService
    {
        private readonly RepoContext repoContext;
        private readonly ISkillService skillService;
        private readonly HeistSettings heistSettings;
        private const string HEIST_STATUS_FAILED = "FAILED";
        private const string HEIST_STATUS_SUCCEEDED = "SUCCEEDED"; 
        private static Random randomGenerator = new Random();

        public HeistService(RepoContext _repoContext, ISkillService _skillService, IOptions<HeistSettings> _heistSettings)
        {
            repoContext = _repoContext;
            skillService = _skillService;
            heistSettings = _heistSettings.Value;
        }

        public async Task<ServiceResult> HeistMembersLevelUp()
        {
            if (heistSettings == null || heistSettings.LevelUpTime == 0)
            {
                return new ServiceResult(false, HeistErrors.NoLevelUpTimeSettings);
            }

            var heistsForLevelUp = await GetHeistsForLevelUp();
            if (!heistsForLevelUp.Any())
            {
                return new ServiceResult(true);
            }

            foreach (var heist in heistsForLevelUp)
            {
                await HeistMembersLevelUp(heist);
            }

            return new ServiceResult(false, HeistErrors.NoLevelUpTimeSettings);
        }

        private async Task<List<Heist>> GetHeistsForLevelUp()
        {
            return await repoContext.Heists.Where(x => x.Status.IsFinished && !x.MemberLevelUpProcessed).ToListAsync();
        }

        private async Task HeistMembersLevelUp(Heist heist)
        {
            var diffInSeconds = (heist.EndTime - heist.StartTime).TotalSeconds;
            var levelUp = (int)Math.Floor(diffInSeconds / heistSettings.LevelUpTime);

            var memberSkills = await repoContext.HeistSkillMemberBrowse
                .Where(x => x.HeistID == heist.ID).Select(x => x.MemberSkill).ToListAsync();
            foreach (var memberSkill in memberSkills)
            {
                memberSkill.Level += levelUp;
                if (memberSkill.Level > 10)
                {
                    memberSkill.Level = 10;
                }
            }

            repoContext.UpdateRange(memberSkills);
            heist.MemberLevelUpProcessed = true;
            repoContext.UpdateRange(heist);
            await repoContext.SaveChangesAsync();
        }

        public async Task<ServiceResult> StartHeist(int id)
        {
            var heist = await repoContext.Heists.SingleOrDefaultAsync(x => x.ID == id);
            if (heist == null)
            {
                return ServiceResult.ErrorResult(HeistErrors.HeistNotFound);
            }
            var heistStatus = await GetHeistStatusById(id);

            if (heistStatus == null || !heistStatus.IsReady) 
            {
                return ServiceResult.ErrorResult(HeistErrors.HeistStatusNotReady);
            }

            var statusInProgress = await repoContext.HeistStatuses.SingleAsync(x => x.IsInProgress);

            heist.StatusID = statusInProgress.ID;
            repoContext.Update(heist);
            await repoContext.SaveChangesAsync();

            return ServiceResult.SuccessResult();
        }

        public async Task<HeistStatusServiceResult> GetHeistStatus(int id)
        {
            var heist = await repoContext.Heists.SingleOrDefaultAsync(x => x.ID == id);
            if (heist == null)
            {
                return new HeistStatusServiceResult(false, HeistErrors.HeistNotFound);
            }
            var heistStatus = await GetHeistStatusById(id);
            return new HeistStatusServiceResult(true)
            {
                StatusDto = new HeistStatusDto
                {
                    StatusName = heistStatus.Name
                }
            };
        }

        private async Task<HeistStatus> GetHeistStatusById(int id)
        {
            return await repoContext.Heists.Where(x => x.ID == id).Select(x => x.Status).SingleAsync();
        }

        public async Task<List<HeistToSkillDto>> GetHeistSkills(int id)
        {
            var heistSkills = await repoContext.HeistToSkills.Include(x => x.Skill).Where(x => x.HeistID == id).ToListAsync();
            return heistSkills.Select(x => x.ToDto()).ToList();
        }

        public async Task<HeistEligibleMembersServiceResult> GetHeistEligibleMembers(int id)
        {
            var heist = await repoContext.Heists.SingleOrDefaultAsync(x => x.ID == id);
            if (heist == null)
            {
                return new HeistEligibleMembersServiceResult(false, HeistErrors.HeistNotFound);
            }

            var heistHasConfirmedMembers = await repoContext.HeistMembers.AnyAsync(x => x.HeistID == id);

            if (heistHasConfirmedMembers)
            {
                return new HeistEligibleMembersServiceResult(false, HeistErrors.MembersAlreadyConfirmed);
            }

            var heistSkills = await GetHeistSkills(id);
            var eligibleMembers = await GetHeistEligibleMembersDto(id);

            var heistEligibleMembersDto = new HeistEligibleMembersDto
            {
                Skills = heistSkills,
                Members = eligibleMembers
            };

            return new HeistEligibleMembersServiceResult(true)
            {
                EligibleMembers = heistEligibleMembersDto
            };
        }

        public async Task<HeistMembersServiceResult> GetHeistMembers(int id)
        {
            var heist = repoContext.Heists.SingleOrDefault(x => x.ID == id);

            if (heist == null)
            {
                return new HeistMembersServiceResult(false, HeistErrors.HeistNotFound);
            }
            else
            {
                var heistStatus = await GetHeistStatusById(id);
                if (heistStatus.IsPlanning)
                {
                    return new HeistMembersServiceResult(false, HeistErrors.HeistInPlaning);
                }
            }
            var members = await GetHeistMembersDto(id);

            return new HeistMembersServiceResult(true)
            {
                Members = members
            };
        }

        private async Task<List<MemberDto>> GetHeistMembersDto(int id)
        {
            var heistMembers = await repoContext.HeistMembers.Where(x => x.HeistID == id)
                .Include(x => x.Member)
                .ThenInclude(x => x.Gender)
                .Include(x => x.Member)
                .ThenInclude(x => x.Status)
                .Include(x => x.Member)
                .ThenInclude(x => x.MainSkill)
                .Include(x => x.Member)
                .ThenInclude(x => x.Skills)
                .ThenInclude(x => x.Skill)
                .Select(x => x.Member).ToListAsync();

            return heistMembers.Select(x => x.ToDto()).ToList();
        }

        private async Task<List<MemberDto>> GetHeistEligibleMembersDto(int id)
        {
            var eligibleMembers = await repoContext.HeistEligibleMemberBrowse.Where(x => x.HeistID == id)
                .Include(x => x.Member)
                .ThenInclude(x => x.Gender)
                .Include(x => x.Member)
                .ThenInclude(x => x.Status)
                .Include(x => x.Member)
                .ThenInclude(x => x.MainSkill)
                .Include(x => x.Member)
                .ThenInclude(x => x.Skills)
                .ThenInclude(x => x.Skill)
                .Select(x => x.Member).ToListAsync();

            return eligibleMembers.Select(x => x.ToDto()).ToList();
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
                return ServiceResult.ErrorResult(HeistErrors.HeistNotFound);
            } else
            {
                if (HeistHasStarted(heist))
                {
                    return ServiceResult.ErrorResult(HeistErrors.HeistHasStarted);
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
            return heist.StartTime <= DateTime.Now;
        }

        private void DeleteHeistSkils(int heistID)
        {
            var existingSkills = repoContext.HeistToSkills.Where(x => x.HeistID == heistID);

            repoContext.HeistToSkills.RemoveRange(existingSkills);
        } 

        public async Task<HeistDto?> GetHeistById(int id)
        {
            var heist = await repoContext.Heists
                .Include(x => x.Status)
                .Include(x => x.Skills)
                .ThenInclude(x => x.Skill)
                .SingleOrDefaultAsync(x => x.ID == id);
            if (heist == null)
            {
                return null;
            }

            return heist.ToDto();
        }

        public async Task<CreateHeistServiceResult> CreateHeist(HeistDto heistDto)
        {
            var errors = await ValidateCreateHeistAsync(heistDto);

            if (errors.Any())
            {
                return new CreateHeistServiceResult(false, "Validation errors", errors);
            }

            var initialStatus = await repoContext.HeistStatuses.SingleAsync(x => x.IsPlanning);

            var heist = new Heist();
            heist.Name = heistDto.Name;
            heist.StartTime = heistDto.StartTime.Value;
            heist.EndTime = heistDto.EndTime.Value;
            heist.Location = heistDto.Location;
            heist.StatusID = initialStatus.ID;

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


        public async Task StartHeists()
        {
            var inProgressStatus = await repoContext.HeistStatuses.SingleAsync(x => x.IsInProgress);
            var now = DateTime.Now.ToUniversalTime();
            var heistThatShouldBeStarted = await repoContext.Heists
                .Where(x => x.Status.IsPlanning && x.StartTime <= now && x.EndTime > now).ToListAsync();
            if (heistThatShouldBeStarted.Any())
            {
                foreach (var heist in heistThatShouldBeStarted)
                {
                    heist.StatusID = inProgressStatus.ID;
                };

                repoContext.UpdateRange(heistThatShouldBeStarted);
                await repoContext.SaveChangesAsync();
            }
        }

        public async Task FinishHeists()
        {
            var finishedStatus = await repoContext.HeistStatuses.SingleAsync(x => x.IsFinished);
            var now = DateTime.Now.ToUniversalTime();
            var heistThatShouldBeFinished = await repoContext.Heists
                .Where(x => x.Status.IsInProgress && x.EndTime < now).ToListAsync();
            if (heistThatShouldBeFinished.Any())
            {
                foreach (var heist in heistThatShouldBeFinished)
                {
                    heist.StatusID = finishedStatus.ID;
                };

                repoContext.UpdateRange(heistThatShouldBeFinished);
                await repoContext.SaveChangesAsync();
            }
        }

        public async Task<ServiceResult> AssignMembersToHeist(int id, AssignMembersToHeistDto assignMembersToHeistDto)
        {
            var heist = repoContext.Heists.SingleOrDefault(x => x.ID == id);

            if (heist == null)
            {
                return ServiceResult.ErrorResult(HeistErrors.HeistNotFound);
            }
            else
            {
                var heistStatus = await GetHeistStatusById(id);
                if (!heistStatus.IsPlanning)
                {
                    return ServiceResult.ErrorResult(HeistErrors.HeistNotInPlaning);
                }
            }

            var heistMembers = new List<HeistMember>();
            var heistEligibleMembers = await repoContext.HeistEligibleMemberBrowse.Where(x => x.HeistID == id).Select(x => x.Member).ToListAsync();
            if (!heistEligibleMembers.Any())
            {
                return ServiceResult.ErrorResult(HeistErrors.HeistDoesntHaveEligibleMembers);
            }

            var distinctMembers = assignMembersToHeistDto.Members.Distinct().ToList();

            foreach (var memberName in distinctMembers)
            {
                var membersToAssign = heistEligibleMembers.Where(x => x.Name == memberName).ToList();
                if(!membersToAssign.Any())
                {
                    return ServiceResult.ErrorResult($"{HeistErrors.MemberIsNotEligibleForThisHeist}: {memberName}");
                }
                foreach (var member in membersToAssign)
                {
                    heistMembers.Add(new HeistMember
                    {
                        HeistID = id,
                        MemberID = member.ID
                    });
                }
            }
            
            var currentHeistMembers = repoContext.HeistMembers.Where(x => x.HeistID == id);
            if (currentHeistMembers.Any())
            {
                repoContext.RemoveRange(currentHeistMembers);
                await repoContext.SaveChangesAsync();
            }

            var readyStatus = await repoContext.HeistStatuses.SingleAsync(x => x.IsReady);

            await repoContext.AddRangeAsync(heistMembers);
            heist.StatusID = readyStatus.ID;
            repoContext.Update(heist);
            await repoContext.SaveChangesAsync();

            return ServiceResult.SuccessResult();
        }


        public async Task<HeistOutcomeServiceResult> GetHeistOutcome(int id)
        {
            using var transaction = await repoContext.Database.BeginTransactionAsync();
            var heist = repoContext.Heists.SingleOrDefault(x => x.ID == id);
            try
            {

                if (heist == null)
                {
                    return new HeistOutcomeServiceResult(false, HeistErrors.HeistNotFound);
                }
                else
                {
                    var heistStatus = await GetHeistStatusById(id);
                    if (!heistStatus.IsFinished)
                    {
                        return new HeistOutcomeServiceResult(false, HeistErrors.HeistNotFinished);
                    }
                }

                var heistAssignedMembersRate = await repoContext.HeistAssignedMembersRateBrowse.SingleOrDefaultAsync(x => x.HeistID == id);
                if (heistAssignedMembersRate == null)
                {
                    return new HeistOutcomeServiceResult(false, HeistErrors.HeistDoesntHaveAnySkill);
                }

                string heistOutcome = string.Empty;

                var expiredStatus = await repoContext.MemberStatuses.SingleAsync(x => x.IsExpired);
                var incarceratedStatus = await repoContext.MemberStatuses.SingleAsync(x => x.IsIncarcerated);

                if (heistAssignedMembersRate.AssignRate < 50)
                {
                    heistOutcome = HEIST_STATUS_FAILED;
                    await SetHeistMembersStatus(id, 1, incarceratedStatus, expiredStatus);
                } else if (heistAssignedMembersRate.AssignRate < 75)
                {
                    var isFailed = RandomTrueOrFalse();
                    if (isFailed)
                    {
                        heistOutcome = HEIST_STATUS_FAILED;
                        await SetHeistMembersStatus(id, 0.66, incarceratedStatus, expiredStatus);
                    } else
                    {
                        heistOutcome = HEIST_STATUS_SUCCEEDED;
                        await SetHeistMembersStatus(id, 0.33, incarceratedStatus, expiredStatus);
                    }
                }
                else if (heistAssignedMembersRate.AssignRate < 100)
                {
                    heistOutcome = HEIST_STATUS_SUCCEEDED;
                    await SetHeistMembersStatus(id, 0.66, incarceratedStatus);
                }
                else 
                {
                    heistOutcome = HEIST_STATUS_SUCCEEDED;
                }

                await transaction.CommitAsync();

                return new HeistOutcomeServiceResult(true)
                {
                    HeistOutcome = new HeistOutcomeDto
                    {
                        Outcome = heistOutcome
                    }
                };
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                return new HeistOutcomeServiceResult(false)
                {
                    ErrorMessage = ex.Message
                };
            }
        }

        private bool RandomTrueOrFalse()
        {
            return randomGenerator.Next(2) == 1;
        }

        private async Task SetHeistMembersStatus(int heistID, double percentage, MemberStatus status1, MemberStatus? status2 = null)
        {    
            var numberOfHeistMembers = await repoContext.HeistMembers.Where(x => x.HeistID == heistID).CountAsync();
            if (numberOfHeistMembers == 0)
            {
                throw new Exception(HeistErrors.HeistDoesntHaveAnyMember);
            }

            int targetCount = (int)Math.Floor(numberOfHeistMembers * percentage);

            // should members be selected random?
            var heistMembers = await repoContext.HeistMembers.Where(x => x.HeistID == heistID).Select(x => x.Member).Take(targetCount).ToListAsync();
            if (heistMembers.Any())
            {
                foreach (var heistMember in heistMembers)
                {
                    var status = status1;
                    if (status2 != null)
                    {
                        var randomIsExpired = RandomTrueOrFalse();
                        status = randomIsExpired ? status1 : status2;
                    }

                    heistMember.StatusID = status.ID;
                }

                repoContext.UpdateRange(heistMembers);

                await repoContext.SaveChangesAsync();
            }
        }
    }
}
