using HopTacDoanhNghiep.Areas.Admin.ViewModels.DonUngTuyen;
using HopTacDoanhNghiep.Enums.HoSo;
using HopTacDoanhNghiep.Models;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public interface IDonUngTuyenAdmin 
    {
        Task<PageResult<DonUngTuyenVM>> GetListDonUngTuyen(int pageInge, int pageSize, string? keyword, HoSoStatus? hoSoStatus, int? maTTD);
        Task<BaseResult<DonUngTuyenVM>> GetDonUngTuyenById(int MaTTD);
    }
}
