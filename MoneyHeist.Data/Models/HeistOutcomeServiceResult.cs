
using MoneyHeist.Data.Dtos.Heist;

namespace MoneyHeist.Data.Models
{
    public class HeistOutcomeServiceResult : ServiceResult
    {
        public HeistOutcomeDto HeistOutcome { get; set; }
        public HeistOutcomeServiceResult(bool success, string? errorMessage = null, IEnumerable<string>? errors = null) : base(success, errorMessage, errors)
        {
        }
    }
}
