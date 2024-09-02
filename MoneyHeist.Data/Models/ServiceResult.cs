using System.Net;

namespace MoneyHeist.Data.Models
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; } = string.Empty;
        public IEnumerable<string>? Errors { get; set; } = new List<string>();

        public ServiceResult(bool success, string? errorMessage = null, IEnumerable<string>? errors = null)
        {
            Success = success;
            ErrorMessage = errorMessage;
            Errors = errors;
        }

        public static ServiceResult SuccessResult()
        {
            return new ServiceResult(true);
        }

        public static ServiceResult ErrorResult(string errorMessage, IEnumerable<string>? errors = null)
        {
            return new ServiceResult(false, errorMessage, errors);
        }
    }
}
