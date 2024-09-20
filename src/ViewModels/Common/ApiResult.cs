namespace src.ViewModels.Common
{
    public class ApiResult<T>
    {
        public bool IsSuccessed { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public ApiResult() { }

        public ApiResult(bool isSuccessed, string? message, T? data)
        {
            IsSuccessed = isSuccessed;
            Message = message;
            Data = data;
        }

        public ApiResult(bool isSuccessed, string? message)
        {
            IsSuccessed = isSuccessed;
            Message = message;
        }
    }
}
