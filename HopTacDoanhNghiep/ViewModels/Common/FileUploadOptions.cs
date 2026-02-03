namespace HopTacDoanhNghiep.ViewModels.Common
{
    public class FileUploadOptions
    {
        public string Folder { get; set; } = string.Empty;
        public long MaxSizeInBytes { get; set; } = 5 * 1024 * 1024; 
        public string[] AllowedExtensions { get; set; } = System.Array.Empty<string>();
        public bool RenameFile { get; set; } = true;
    }
}
