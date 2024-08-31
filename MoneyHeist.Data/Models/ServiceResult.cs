namespace MoneyHeist.Data.Models
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string? ErrorMessage { get; set; }
        public IEnumerable<string>? Errors { get; set; }

        public ServiceResult(bool success, T data, string? errorMessage = null, IEnumerable<string>? errors = null)
        {
            Success = success;
            Data = data;
            ErrorMessage = errorMessage;
            Errors = errors;
        }

        public static ServiceResult<T> SuccessResult(T data)
        {
            return new ServiceResult<T>(true, data);
        }

        public static ServiceResult<T> ErrorResult(string errorMessage, IEnumerable<string>? errors = null)
        {
            return new ServiceResult<T>(false, default, errorMessage, errors);
        }
    }
}
