using HopTacDoanhNghiep.Areas.Admin.ViewModels.SinhVien;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public interface ISinhVienAdmin
    {
        Task<PageResult<SinhVienVM>> GetListSinhVien(int pageIndex = 1, int pageSize = 10, string? keyword = null, string? khoa = null, string? chuyenNganh = null);
        Task<BaseResult<SinhVienVM>> GetSinhVienById(Guid id);
    }
}
