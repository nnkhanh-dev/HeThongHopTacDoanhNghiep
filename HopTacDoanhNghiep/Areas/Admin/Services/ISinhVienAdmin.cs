using HopTacDoanhNghiep.Areas.Admin.ViewModels.SinhVien;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public interface ISinhVienAdmin
    {
        Task<PageResult<SinhVienVM>> GetListSinhVien(int pageIndex, int pageSize, string? keyword);
        Task<BaseResult<SinhVienVM>> GetSinhVienByMaSV (string maSV);
    }
}
