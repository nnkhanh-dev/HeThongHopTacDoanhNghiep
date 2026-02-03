namespace HopTacDoanhNghiep.Services
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using HopTacDoanhNghiep.ViewModels.Common;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class FileStorageService : IFileStorage
    {
        private readonly IWebHostEnvironment _env;

        public FileStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<FileUploadResult> UploadAsync(IFormFile file, FileUploadOptions options, CancellationToken cancellationToken = default)
        {
            if (file == null || file.Length == 0)
                return FileUploadResult.Fail("No file provided.");

            options ??= new FileUploadOptions();

            if (options.MaxSizeInBytes > 0 && file.Length > options.MaxSizeInBytes)
                return FileUploadResult.Fail($"File size exceeds limit ({options.MaxSizeInBytes} bytes).");

            var ext = Path.GetExtension(file.FileName) ?? string.Empty;
            if (options.AllowedExtensions != null && options.AllowedExtensions.Length > 0)
            {
                if (!options.AllowedExtensions.Any(a => string.Equals(a, ext, StringComparison.OrdinalIgnoreCase)))
                    return FileUploadResult.Fail("File extension not allowed.");
            }

            var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var folder = string.IsNullOrWhiteSpace(options.Folder) ? string.Empty : options.Folder.Trim('/','\\');
            var uploadsRoot = Path.Combine(webRoot, folder);
            if (!Directory.Exists(uploadsRoot))
                Directory.CreateDirectory(uploadsRoot);

            var fileName = options.RenameFile ? (Guid.NewGuid().ToString() + ext) : Path.GetFileName(file.FileName);
            var physicalPath = Path.Combine(uploadsRoot, fileName);

            using (var stream = new FileStream(physicalPath, FileMode.Create))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }

            var relative = "/" + Path.Combine(folder, fileName).Replace('\\', '/').TrimStart('/');
            return FileUploadResult.Success(relative);
        }

        public Task DeleteAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return Task.CompletedTask;

            var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var trimmed = filePath.TrimStart('~', '/').Replace('/', Path.DirectorySeparatorChar);
            var physical = Path.Combine(webRoot, trimmed);
            if (File.Exists(physical))
                File.Delete(physical);

            return Task.CompletedTask;
        }
    }
}
