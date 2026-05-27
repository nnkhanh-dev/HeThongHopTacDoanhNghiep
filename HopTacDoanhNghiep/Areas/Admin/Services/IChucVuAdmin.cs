using HopTacDoanhNghiep.Areas.Admin.ViewModels.ChucVu;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public interface IChucVuAdmin
    {
        Task<PageResult<ChucVuVM>> GetChucVuAsync(int pageIndex, int pageSize, string? keyword);
    }
}
