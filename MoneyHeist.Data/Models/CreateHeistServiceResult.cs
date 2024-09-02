
namespace MoneyHeist.Data.Models
{
    public class CreateHeistServiceResult : ServiceResult
    {
        public int HeistID { get; set; }
        public CreateHeistServiceResult(bool success, string? errorMessage = null, IEnumerable<string>? errors = null) : base(success, errorMessage, errors)
        {
        }
    }
}
