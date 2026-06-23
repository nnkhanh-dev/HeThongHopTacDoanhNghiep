using HopTacDoanhNghiep.Areas.Company.ViewModels.DonUngTuyen;
using HopTacDoanhNghiep.ViewModels.Common;
using HopTacDoanhNghiep.Enums.HoSo;

namespace HopTacDoanhNghiep.Areas.Company.Services
{
    public interface IDonUngTuyenCompany
    {
        Task<PageResult<DonUngTuyenVM>> GetListDonUngTuyen(int pageInge, int pageSize, string? keyword, string MaDoanhNghiep, HoSoStatus? hoSoStatus, int? maTTD);
        Task<BaseResult<DonUngTuyenVM>> GetDonUngTuyenById(int MaTTD, string MaDoanhNghiep);
        Task<BaseResult> UpdateTrangThaiDonUngTuyen(int MaUT, HoSoStatus trangThai, string MaDoanhNghiep);
    }
}
