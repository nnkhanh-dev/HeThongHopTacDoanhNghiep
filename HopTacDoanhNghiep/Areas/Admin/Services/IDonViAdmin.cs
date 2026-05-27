using HopTacDoanhNghiep.Areas.Admin.ViewModels.DonVi;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public interface IDonViAdmin
    {
        Task<PageResult<DonViVM>> GetDonViAsync(int pageIndex, int pageSize, string? keyword);
    }
}
