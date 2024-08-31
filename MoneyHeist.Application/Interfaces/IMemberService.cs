using MoneyHeist.Data.Dtos.Member;
using MoneyHeist.Data.Models;

namespace MoneyHeist.Application.Interfaces
{
    public interface IMemberService
    {
        public Task<ServiceResult<MemberDto>> CreateMember(MemberDto memberDto);
        public Task<ServiceResult<UpdateMemberSkillsDto>> UpdateMemberSkills(int memberID, UpdateMemberSkillsDto updateMemberSkillsDto);
    }
}
