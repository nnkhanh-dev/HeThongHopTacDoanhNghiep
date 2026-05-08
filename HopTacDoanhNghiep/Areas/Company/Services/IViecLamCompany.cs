using HopTacDoanhNghiep.Areas.Company.ViewModels.ViecLam;
using HopTacDoanhNghiep.Enums.ViecLam;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Company.Services
{
    public interface IViecLamCompany
    {
        Task<PageResult<VietLamVM>> GetListViecLam(
            string doanhNghiepId,
            int pageIndex,
            int pageSize,
            string? keyword = null,
            string? linhVuc = null,
            ViecLamStatus? status = null,
            ViecLamType? loaiViecLam = null,
            DoiTuongUngTuyen? doiTuongUngTuyen = null,
            TrinhDoType? trinhDo = null,
            long? luongMin = null,
            long? luongMax = null,
            bool? conHieuLuc = null,
            bool? sapXepLuongToiDa = null,
            string? sapXepTheo = null
        );
        Task<BaseResult<VietLamVM>> GetViecLamById(int id);
        Task<BaseResult> CreateViecLam(VietLamCreateVM viecLam);
        Task<BaseResult> EditViecLam(int id, VietLamEditVM viecLam);
        Task<BaseResult> DeleteViecLam(int id, string deletedBy);
    }
}
