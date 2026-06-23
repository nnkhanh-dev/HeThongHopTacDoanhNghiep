using HopTacDoanhNghiep.Areas.Student.ViewModels.DonUngTuyen;
using HopTacDoanhNghiep.Enums.HoSo;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Student.Services
{
    public interface IDonUngTuyenStudent
    {
        Task<PageResult<DonUngTuyenVM>> GetListDonUngTuyen(int pageInge, int pageSize, string? keyword, string MaSinhVien, HoSoStatus? trangthai);
        Task<BaseResult<DonUngTuyenVM>> GetDonUngTuyenById(int MaTTD, string MaSinhVien);
        Task<BaseResult> WithdrawApplication(int MaUT, string MaSinhVien);
        Task<BaseResult> ApplyDonUngTuyen (DonUngTuyenCreateVM donUngTuyen);
    }
}
