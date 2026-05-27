using HopTacDoanhNghiep.Areas.Student.ViewModels.ViecLam;
using HopTacDoanhNghiep.Enums.ViecLam;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Student.Services
{
    public interface ITinTuyenDungStudent
    {
        Task<PageResult<TinTuyenDungVM>> GetListTinTuyenDung(
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
        Task<BaseResult<TinTuyenDungVM>> GetTinTuyenDungBySlug(string slug);
    }
}
