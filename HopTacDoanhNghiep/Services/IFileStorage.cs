using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Services
{
    public interface IFileStorage
    {
        Task<FileUploadResult> UploadAsync(IFormFile file, FileUploadOptions options, CancellationToken cancellationToken = default);
        Task DeleteAsync(string filePath);
    }
}
