using HopTacDoanhNghiep.Areas.Admin.ViewModels.NhapDuLieu;
using HopTacDoanhNghiep.Enums.NhapDuLieu;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public interface INhapDuLieuAdmin
    {
        Task<PageResult<LichSuNhapDuLieuVM>> GetListLichSuNhapDuLieu(NhapDuLieuType type, int pageIndex = 1, int pageSize = 10, string? keyword = null, NhapDuLieuStatus? status = null);
        Task<BaseResult> UploadSinhVienExcel(IFormFile file, string uploadById);
        Task<BaseResult> UploadDoanhNghiepExcel(IFormFile file, string uploadById);
    }
}