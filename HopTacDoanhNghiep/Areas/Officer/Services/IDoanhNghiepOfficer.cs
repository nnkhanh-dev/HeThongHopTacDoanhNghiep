using HopTacDoanhNghiep.Areas.Officer.ViewModels;
using HopTacDoanhNghiep.Enums.HopTac;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Officer.Services
{
    public interface IDoanhNghiepOfficer
    {
        Task<PageResult<DangKyDoanhNghiepVM>> GetListDangKyDoanhNghiep(int pageIndex, int pageSize, string? keyword, string MaCB);
        Task<PageResult<DoanhNghiepVM>> GetListDoanhNghiep(int pageIndex, int pageSize, string keyword);
        Task<BaseResult<DoanhNghiepVM>> GetDoanhNghiepByMaDN(string MaDN);
        Task<BaseResult> UpdateTrangThaiHopTac(string MaDN, HopTacDoanhNghiepStatus trangThai, string MaCB);

        // Phần hợp tác với các khoa
        Task<PageResult<HopTacDonViVM>> GetListHopTacDonVi(int pageIndex, int pageSize, string? keyword, string MaCB);
        Task<BaseResult<HopTacDonViVM>> GetHopTacDonViByMaHTDV (int MaHTDV);
        Task<BaseResult> UpdateTrangThaiHopTacDV(int maHTDV, HopTacDonViStatus trangThai, string MaCB);

        // Cán bộ
        Task<BaseResult<CanBoVM>> GetCanBoInfo(string MaCanBo);
        Task<BaseResult> UpdateCanBoInfo(string MaCanBo, CanBoUpdateVM updateVM);
    }
}
