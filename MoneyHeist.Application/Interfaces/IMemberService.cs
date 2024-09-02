using MoneyHeist.Data.Dtos.Member;
using MoneyHeist.Data.Models;

namespace MoneyHeist.Application.Interfaces
{
    public interface IMemberService
    {
        public Task<CreateMemberServiceResult> CreateMember(MemberDto memberDto);
        public Task<ServiceResult> UpdateMemberSkills(int memberID, MemberSkillsDto updateMemberSkillsDto);
        public Task<ServiceResult> DeleteMemberSkills(int memberID, string skillName);
    }
}
