using HopTacDoanhNghiep.Areas.Company.ViewModels.DoanhNghiep;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Company.Services
{
    public interface IDoanhNghiepCompany
    {
        Task<BaseResult<DoanhNghiepVM>> GetDoanhNghiepInfo(string MaDoanhNghiep);
        Task<BaseResult> UpdateDoanhNghiepInfo(string MaDoanhNghiep, DoanhNghiepUpdateVM updateVM);
        Task<BaseResult<NguoiDaiDienVM>> GetNguoiDaiDienInfo(string MaDoanhNghiep);
        Task<BaseResult> UpdateNguoiDaiDienInfo(string MaDoanhNghiep, NguoiDaiDienUpdateVM updateVM);
    }
}
