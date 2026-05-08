using HopTacDoanhNghiep.Enums.ViecLam;
using HopTacDoanhNghiep.Models;
using HopTacDoanhNghiep.ViewModels.Common;
using HopTacDoanhNghiep.ViewModels.ViecLam;

namespace HopTacDoanhNghiep.Services
{
    public interface IViecLam
    {
        Task<PageResult<ViecLamVM>> GetListViecLam(
            int pageIndex = 1,
            int pageSize = 10,
            string? keyword = null,
            string? linhVucSlug = null,
            ViecLamType? loaiViecLam = null,
            TrinhDoType? trinhDo = null,
            DoiTuongUngTuyen? doiTuongUngTuyen = null,
            double? luongMin = null,
            double? luongMax = null,
            bool? sapXepLuongToiDa = null,
            string? sapXepTheo = null);

        Task<BaseResult<ViecLamVM>> GetViecLamBySlug(string slug);
        Task<PageResult<ViecLamVM>> GetRelatedViecLam(int pageIndex, int pageSize, string viecLamSlug, string? keyword, string? linhVucSlug);
    }
}
