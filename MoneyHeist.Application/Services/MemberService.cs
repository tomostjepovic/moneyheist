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
            if (memberDto.Name == null || memberDto.Name.Length == 0)
            {
                return ServiceResult<MemberDto>.ErrorResult("Name is required");
            }

            if (memberDto.Status == null || memberDto.Status.Length == 0)
            {
                return ServiceResult<MemberDto>.ErrorResult("Status is required");
            }

            if (memberDto.Sex == null || memberDto.Sex.Length == 0)
            {
                return ServiceResult<MemberDto>.ErrorResult("Sex is required");
            }

            if (memberDto.Email == null || memberDto.Email.Length == 0)
            {
                return ServiceResult<MemberDto>.ErrorResult("Email is required");
            }

            if (memberDto.Skills == null || !memberDto.Skills.Any())
            {
                return ServiceResult<MemberDto>.ErrorResult("At least one skill is required");
            }

            if (memberDto.MainSkill != null)
            {
                var mainSkill = memberDto.Skills.Where(x => x.Name == memberDto.MainSkill).FirstOrDefault();
                if (mainSkill == null)
                {
                    return ServiceResult<MemberDto>.ErrorResult("Main skill references skill that is not in member's skill list");
                }
            }

            if (await MemberWithEmailExistsAsync(memberDto.Email))
            {
                return ServiceResult<MemberDto>.ErrorResult("Member with same email exists");
            }

            var gender = await repoContext.Genders.FirstOrDefaultAsync(x => x.Name == memberDto.Sex);
            if (gender == null)
            {
                var allGenders = (await repoContext.Genders.ToListAsync()).Select(x => x.Name).ToList();
                return ServiceResult<MemberDto>.ErrorResult($"Please send correct sex. Available values are: {string.Join(", ", allGenders)}");
            }

            var status = await repoContext.MemberStatuses.FirstOrDefaultAsync(x => x.Name == memberDto.Status);
            if (status == null)
            {
                var allMemberStatuses = (await repoContext.MemberStatuses.ToListAsync()).Select(x => x.Name).ToList();
                return ServiceResult<MemberDto>.ErrorResult($"Please send correct status. Available values are: {string.Join(", ", allMemberStatuses)}");
            }

            var member = new Member();
            member.Name = memberDto.Name;
            member.Email = memberDto.Email;
            member.StatusID = status.ID;
            member.GenderID = gender.ID;

            repoContext.Add(member);
            await repoContext.SaveChangesAsync();

            return ServiceResult<MemberDto>.SuccessResult(memberDto);
        }

        private async Task<bool> MemberWithEmailExistsAsync(string email)
        {
            return await repoContext.Members.AnyAsync(x => x.Email == email);
        }
    }
}
