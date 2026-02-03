namespace HopTacDoanhNghiep.ViewModels.Common
{
    public class BaseResult<T>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; } 

        public static BaseResult<T> Success(T? data, string message = "")
            => new() { IsSuccess = true, Data = data, Message = message };

        public static BaseResult<T> Fail(string message)
            => new() { IsSuccess = false, Message = message };
    }

    public class BaseResult
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }

        public static BaseResult Success(string? message = null)
        => new() { IsSuccess = true, Message = message };

        public static BaseResult Fail(string message)
            => new() { IsSuccess = false, Message = message };
    }

}
