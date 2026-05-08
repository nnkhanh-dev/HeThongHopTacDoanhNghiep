using HopTacDoanhNghiep.Areas.Admin.ViewModels.DanhMucBaiViet;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public interface IDanhMucBaiVietAdmin
    {
        Task<PageResult<DanhMucBaiVietVM>> GetListDanhMucBaiViet(int pageIndex, int pageSize, string? keyword);
        Task<BaseResult<DanhMucBaiVietVM>> GetDanhMucBaiVietById(int id);
        Task<BaseResult> CreateDanhMucBaiViet(DanhMucBaiVietCreateVM danhMuc);
        Task<BaseResult> EditDanhMucBaiViet(int id, DanhMucBaiVietEditVM danhMuc);
        Task<BaseResult> DeleteDanhMucBaiViet(int id, string deletedBy);
    }
}
