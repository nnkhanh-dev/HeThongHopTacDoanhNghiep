using HopTacDoanhNghiep.Areas.Admin.ViewModels.DoanhNghiep;
using HopTacDoanhNghiep.Areas.Admin.ViewModels.SinhVien;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public interface IDoanhNghiepAdmin
    {
        Task<PageResult<DoanhNghiepVM>> GetListDoanhNghiep(int pageIndex = 1, int pageSize = 10, string? keyword = null);
        Task<BaseResult<DoanhNghiepVM>> GetDoanhNghiepById(Guid id);
    }
}
