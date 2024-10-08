﻿using Microsoft.EntityFrameworkCore;
using MoneyHeist.Application.Interfaces;
using MoneyHeist.Application.Mappers;
using MoneyHeist.Data.Dtos.Member;
using MoneyHeist.Data.Entities;
using MoneyHeist.Data.ErrorCodes;
using MoneyHeist.Data.Models;
using MoneyHeist.DataAccess;

namespace MoneyHeist.Application.Services
{
    public class MemberService : IMemberService
    {
        private readonly RepoContext repoContext;

        private readonly ISkillService skillService;

        public MemberService(RepoContext _repoContext, ISkillService _skillService)
        {
            repoContext = _repoContext;
            skillService = _skillService;
        }

        public async Task<MemberDto?> GetMemberById(int id)
        {
            var member = await repoContext.Members
                .Include(x => x.MainSkill)
                .Include(x => x.Status)
                .Include(x => x.Gender)
                .Include(x => x.Skills)
                .ThenInclude(x => x.Skill).SingleOrDefaultAsync(x => x.ID == id);
            if (member == null) 
            {
                return null;
            }

            return member.ToDto();
        }

        public async Task<ServiceResult> DeleteMemberSkills(int memberID, string skillName) 
        {
            var memberSkill = await repoContext.MemberToSkills.SingleOrDefaultAsync(x => x.MemberID == memberID && x.Skill.Name.ToLower() == skillName.ToLower());

            if (memberSkill == null)
            {
                return ServiceResult.ErrorResult(HeistErrors.MemberOrMemberSkillNotFound);
            }

            if (memberSkill != null)
            {
                repoContext.MemberToSkills.Remove(memberSkill);
                await repoContext.SaveChangesAsync();
            }

            return ServiceResult.SuccessResult();
        }

        private bool MemberSkillsHasDuplicates(List<MemberToSkillDto> memberSkills)
        {
            var skills = memberSkills.Select(x => x.Name.ToLower()).ToList();
            return skills.GroupBy(x => x).Any(grp => grp.Count() > 1);
        }

        public async Task<ServiceResult> UpdateMemberSkills(int memberID, MemberSkillsDto updateMemberSkillsDto)
        {
            var member = await repoContext.Members.SingleOrDefaultAsync(x => x.ID == memberID);
            if (member == null)
            {
                return ServiceResult.ErrorResult(HeistErrors.MemberNotFound);
            }

            var errors = await ValidateUpdateMemberSkills(memberID, updateMemberSkillsDto);
            if (errors.Any())
            {
                return ServiceResult.ErrorResult("Validation errors", errors);
            }

            if (updateMemberSkillsDto.Skills != null && updateMemberSkillsDto.Skills.Any())
            {
                var existingSkills = repoContext.MemberToSkills.Where(x => x.MemberID == memberID);

                repoContext.MemberToSkills.RemoveRange(existingSkills);

                foreach (var memberToSkillDto in updateMemberSkillsDto.Skills)
                {
                    await InsertMemberToSkill(memberID, memberToSkillDto);
                }
            }

            if (!string.IsNullOrEmpty(updateMemberSkillsDto.MainSkill))
            {
                var mainSkill = await repoContext.Skills.SingleAsync(x => x.Name.ToLower() == updateMemberSkillsDto.MainSkill.ToLower());
                member.MainSkillID = mainSkill.ID;

                repoContext.Update(member);
                await repoContext.SaveChangesAsync();
            }

            return ServiceResult.SuccessResult();
        }

        private async Task<List<string>> ValidateUpdateMemberSkills(int memberID, MemberSkillsDto updateMemberSkillsDto)
        {
            List<string> errors = new List<string>();            

            if (string.IsNullOrEmpty(updateMemberSkillsDto.MainSkill) && (updateMemberSkillsDto.Skills == null || !updateMemberSkillsDto.Skills.Any()))
            {
                errors.Add("At least one skill or main skill must be provided");
            }

            if (updateMemberSkillsDto.Skills != null && updateMemberSkillsDto.Skills.Any())
            {
                var skillsErrors = ValidateMemberSkills(updateMemberSkillsDto.Skills);
                if (skillsErrors != null)
                {
                    errors.AddRange(skillsErrors);
                }
            }

            if (!string.IsNullOrEmpty(updateMemberSkillsDto.MainSkill))
            {
                if (updateMemberSkillsDto.Skills != null && updateMemberSkillsDto.Skills.Any())
                {
                    var mainSkillExistsInNewSkills = updateMemberSkillsDto.Skills.Any(x => x.Name.ToLower() == updateMemberSkillsDto.MainSkill.ToLower());
                    if (!mainSkillExistsInNewSkills)
                    {
                        errors.Add("Main skill doesnt exist in new skill set");
                    }
                }
                else
                {
                    var mainSkillExistsInExistingMembersSkills = await repoContext.MemberToSkills.AnyAsync(
                        x => x.MemberID == memberID && x.Skill.Name.ToLower() == updateMemberSkillsDto.MainSkill.ToLower());
                    if (!mainSkillExistsInExistingMembersSkills)
                    {
                        errors.Add("Main skill doesnt exist in existing skill set");
                    }
                }
            }

            return errors;

        }

        public async Task<CreateMemberServiceResult> CreateMember(MemberDto memberDto)
        {
            var errors = await ValidateCreateMemberAsync(memberDto);

            if (errors.Any())
            {
                return new CreateMemberServiceResult(false, "Validation errors", errors);
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

            if (!string.IsNullOrEmpty(memberDto.MainSkill))
            {
                var mainSkill = await repoContext.Skills.SingleAsync(x => x.Name.ToLower() == memberDto.MainSkill.ToLower());
                member.MainSkillID = mainSkill.ID;

                repoContext.Update(member);
                await repoContext.SaveChangesAsync();
            }

            return new CreateMemberServiceResult(true)
            {
                MemberID = member.ID,
            };
        }

        private async Task<List<string>> ValidateCreateMemberAsync(MemberDto memberDto)
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrWhiteSpace(memberDto.Name))
            {
                errors.Add("Name is required");
            }

            if (string.IsNullOrWhiteSpace(memberDto.Status))
            {
                errors.Add("Status is required");
            }

            if (memberDto.Sex == null || memberDto.Sex.Length == 0)
            {
                errors.Add("Sex is required");
            }

            if (string.IsNullOrWhiteSpace(memberDto.Email))
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

            var skillsErrors = ValidateMemberSkills(memberDto.Skills);
            if (skillsErrors != null)
            {
                errors.AddRange(skillsErrors);
            }            

            return errors;
        }

        private List<string> ValidateMemberSkills(List<MemberToSkillDto> memberSkills)
        {
            List<string> errors = new List<string>();
            var hasDuplicateSkils = MemberSkillsHasDuplicates(memberSkills);
            if (hasDuplicateSkils)
            {
                errors.Add($"Member cannot have duplicate skills");
            }

            var skillWithIncorectLevelExists = MemberSkillsHasInvalidLevel(memberSkills);
            if (skillWithIncorectLevelExists)
            {
                errors.Add($"Skill level can only contain '*' characters with a maximum of 10.");
            }

            return errors;
        }

        public bool MemberSkillsHasInvalidLevel(List<MemberToSkillDto> memberSkills)
        {
            return memberSkills.Any(x => !x.LevelIsValid);
        }

        private async Task InsertMemberToSkill(int memberID, MemberToSkillDto memberToSkillDto)
        {
            var skill = await skillService.GetOrInsertSkillIfNotExists(memberToSkillDto.Name);
            var memberToSkill = new MemberToSkill
            {
                MemberID = memberID,
                SkillID = skill.ID,
                Level = memberToSkillDto.LevelNumeric
            };

            await repoContext.AddAsync(memberToSkill);
            await repoContext.SaveChangesAsync();
        }

        private async Task<bool> MemberWithEmailExistsAsync(string email)
        {
            return await repoContext.Members.AnyAsync(x => x.Email == email);
        }
    }
}
