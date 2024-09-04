
using MoneyHeist.Data.Dtos.Member;

namespace MoneyHeist.Data.Models
{
    public class HeistMembersServiceResult : ServiceResult
    {
        public List<MemberDto> Members { get; set; }
        public HeistMembersServiceResult(bool success, string? errorMessage = null, IEnumerable<string>? errors = null) : base(success, errorMessage, errors)
        {
        }
    }
}
