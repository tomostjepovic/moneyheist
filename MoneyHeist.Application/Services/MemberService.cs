using Microsoft.EntityFrameworkCore;
using MoneyHeist.Application.Interfaces;
using MoneyHeist.Data.Dtos.Member;
using MoneyHeist.Data.Entities;
using MoneyHeist.Data.Models;
using MoneyHeist.DataAccess;

namespace MoneyHeist.Application.Services
{
    public class MemberService : IMemberService
    {
        private readonly RepoContext repoContext;

        public MemberService(RepoContext _repoContext)
        {
            repoContext = _repoContext;
        }

        public async Task<ServiceResult<MemberDto>> CreateMember(MemberDto memberDto)
        {
            var errors = await ValidateCreateMemberAsync(memberDto);

            if (errors.Any())
            {
                return ServiceResult<MemberDto>.ErrorResult("Validation errors", errors);
            }

            var gender = await repoContext.Genders.FirstOrDefaultAsync(x => x.Name == memberDto.Sex);
            var status = await repoContext.MemberStatuses.FirstOrDefaultAsync(x => x.Name == memberDto.Status);

            var member = new Member();
            member.Name = memberDto.Name;
            member.Email = memberDto.Email;
            member.StatusID = status.ID;
            member.GenderID = gender.ID;

            await repoContext.AddAsync(member);
            await repoContext.SaveChangesAsync();

            memberDto.ID = member.ID;

            foreach (var memberToSkillDto in memberDto.Skills)
            {
                await InsertMemberToSkill(member.ID, memberToSkillDto);
            }

            if (memberDto.MainSkill != null)
            {
                var mainSkill = await repoContext.Skills.SingleAsync(x => x.Name.ToLower() == memberDto.MainSkill.ToLower());
                member.MainSkillID = mainSkill.ID;

                repoContext.Update(member);
                await repoContext.SaveChangesAsync();

            }

            return ServiceResult<MemberDto>.SuccessResult(memberDto);
        }

        private async Task<List<string>> ValidateCreateMemberAsync(MemberDto memberDto)
        {
            List<string> errors = new List<string>();
            if (memberDto.Name == null || memberDto.Name.Length == 0)
            {
                errors.Add("Name is required");
            }

            if (memberDto.Status == null || memberDto.Status.Length == 0)
            {
                errors.Add("Status is required");
            }

            if (memberDto.Sex == null || memberDto.Sex.Length == 0)
            {
                errors.Add("Sex is required");
            }

            if (memberDto.Email == null || memberDto.Email.Length == 0)
            {
                errors.Add("Email is required");
            }

            if (memberDto.Skills == null || !memberDto.Skills.Any())
            {
                errors.Add("At least one skill is required");
            }

            if (memberDto.MainSkill != null)
            {
                var mainSkill = memberDto.Skills.Where(x => x.Name == memberDto.MainSkill).FirstOrDefault();
                if (mainSkill == null)
                {
                    errors.Add("Main skill references skill that is not in member's skill list");
                }
            }

            if (await MemberWithEmailExistsAsync(memberDto.Email))
            {
                errors.Add("Member with same email exists");
            }

            var gender = await repoContext.Genders.FirstOrDefaultAsync(x => x.Name == memberDto.Sex);
            if (gender == null)
            {
                var allGenders = (await repoContext.Genders.ToListAsync()).Select(x => x.Name).ToList();
                errors.Add($"Please send correct sex. Available values are: {string.Join(", ", allGenders)}");
            }

            var status = await repoContext.MemberStatuses.FirstOrDefaultAsync(x => x.Name == memberDto.Status);
            if (status == null)
            {
                var allMemberStatuses = (await repoContext.MemberStatuses.ToListAsync()).Select(x => x.Name).ToList();
                errors.Add($"Please send correct status. Available values are: {string.Join(", ", allMemberStatuses)}");
            }

            var skills = memberDto.Skills.Select(x => x.Name.ToLower()).ToList();
            var hasDuplicateSkils = skills.GroupBy(x => x).Any(grp => grp.Count() > 1);
            if (hasDuplicateSkils)
            {
                errors.Add($"Member cannot have duplicate skills");
            }

            var skillWithIncorectLevelExists = memberDto.Skills.Any(x => !x.LevelIsValid);
            if (skillWithIncorectLevelExists)
            {
                errors.Add($"Skill level can only contain '*' characters with a maximum of 10.");
            }

            return errors;
        }

        private async Task InsertMemberToSkill(int memberID, MemberToSkillDto memberToSkillDto)
        {

            var skill = await GetOrInsertSkillIfNotExists(memberToSkillDto.Name);
            var memberToSkill = new MemberToSkill
            {
                MemberID = memberID,
                SkillID = skill.ID,
                Level = memberToSkillDto.LevelNumeric
            };


            await repoContext.AddAsync(memberToSkill);
            await repoContext.SaveChangesAsync();

        }

        private async Task<Skill> GetOrInsertSkillIfNotExists(string name)
        {
            var skill = await repoContext.Skills.SingleOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
            if (skill == null)
            {
                skill = new Skill
                {
                    Name = name
                };

                await repoContext.AddAsync(skill);
                await repoContext.SaveChangesAsync();

                return skill;
            }

            return skill;
        }



        private async Task<bool> MemberWithEmailExistsAsync(string email)
        {
            return await repoContext.Members.AnyAsync(x => x.Email == email);
        }
    }
}
