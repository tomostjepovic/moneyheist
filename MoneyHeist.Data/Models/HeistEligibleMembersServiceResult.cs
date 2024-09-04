
using MoneyHeist.Data.Dtos.Heist;

namespace MoneyHeist.Data.Models
{
    public class HeistEligibleMembersServiceResult : ServiceResult
    {
        public HeistEligibleMembersDto EligibleMembers { get; set; }
        public HeistEligibleMembersServiceResult(bool success, string? errorMessage = null, IEnumerable<string>? errors = null) : base(success, errorMessage, errors)
        {
        }
    }
}
