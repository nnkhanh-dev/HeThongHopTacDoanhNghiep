namespace HopTacDoanhNghiep.ViewModels.Common
{
    public class FileResult
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public byte[] FileContent { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;

        public static FileResult Success(byte[] fileContent, string contentType, string fileName)
            => new() { IsSuccess = true, FileContent = fileContent, ContentType = contentType, FileName = fileName };

        public static FileResult Fail(string message)
            => new() { IsSuccess = false, Message = message };
    }
}
