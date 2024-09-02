
namespace MoneyHeist.Data.Models
{
    public class CreateMemberServiceResult : ServiceResult
    {
        public int MemberID { get; set; }
        public CreateMemberServiceResult(bool success, string? errorMessage = null, IEnumerable<string>? errors = null) : base(success, errorMessage, errors)
        {
        }
    }
}
