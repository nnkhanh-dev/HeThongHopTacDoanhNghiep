using HopTacDoanhNghiep.Enums.ViecLam;
using HopTacDoanhNghiep.Models;
using HopTacDoanhNghiep.ViewModels.Common;
using HopTacDoanhNghiep.ViewModels.ViecLam;

namespace HopTacDoanhNghiep.Services
{
    public interface ITinTuyenDung
    {
        Task<PageResult<ViecLamVM>> GetListTinTuyenDung(
            int pageIndex = 1,
            int pageSize = 10,
            string? keyword = null,
            ViecLamType? loaiViecLam = null,
            TrinhDoType? trinhDo = null,
            DoiTuongUngTuyen? doiTuongUngTuyen = null,
            double? luongMin = null,
            double? luongMax = null,
            bool? sapXepLuongToiDa = null,
            string? sapXepTheo = null);

        Task<BaseResult<ViecLamVM>> GetTinTuyenDungBySlug(string slug);
        Task<PageResult<ViecLamVM>> GetRelatedTinTuyenDung(int pageIndex, int pageSize, string viecLamSlug, string? keyword);
    }
}
