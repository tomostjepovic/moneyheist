
using MoneyHeist.Data.Dtos.Heist;

namespace MoneyHeist.Data.Models
{
    public class HeistStatusServiceResult : ServiceResult
    {
        public HeistStatusDto StatusDto { get; set; }
        public HeistStatusServiceResult(bool success, string? errorMessage = null, IEnumerable<string>? errors = null) : base(success, errorMessage, errors)
        {
        }
    }
}
