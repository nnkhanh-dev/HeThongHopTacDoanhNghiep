namespace HopTacDoanhNghiep.ViewModels.Common
{
    public class FileUploadResult
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public string? FilePath { get; set; }
        public static FileUploadResult Success(string filePath, string message = "")
            => new() { IsSuccess = true, FilePath = filePath, Message = message };
        public static FileUploadResult Fail(string message)
            => new() { IsSuccess = false, Message = message };
    }
}
