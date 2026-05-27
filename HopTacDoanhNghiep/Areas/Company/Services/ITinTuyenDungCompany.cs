using HopTacDoanhNghiep.Areas.Company.ViewModels.ViecLam;
using HopTacDoanhNghiep.Enums.ViecLam;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Company.Services
{
    public interface ITinTuyenDungCompany
    {
        Task<PageResult<TinTuyenDungVM>> GetListTinTuyenDung(
            string MaDoanhNghiep,
            int pageIndex,
            int pageSize,
            string? keyword = null,
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
        Task<BaseResult<TinTuyenDungVM>> GetTinTuyenDungById(int id);
        Task<BaseResult> CreateTinTuyenDung(TinTuyenDungCreateVM viecLam);
        Task<BaseResult> EditTinTuyenDung(int id, TinTuyenDungEditVM viecLam);
        Task<BaseResult> DeleteTinTuyenDung(int id, string deletedBy);
    }
}
