using HopTacDoanhNghiep.Areas.Student.ViewModels.BaiViet;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Student.Services
{
    public interface IBaiVietStudent
    {
        public Task<BaseResult<DanhMucBaiVietVM>> GetDanhMucBySlug(string slug);
        public Task<PageResult<BaiVietVM>> GetListBaiViet(int pageIndex, int pageSize, string? keyword, string? danhMucSlug);
        public Task<BaseResult<BaiVietVM>> GetBaiVietBySlug(string slug);
        public Task<PageResult<BaiVietVM>> GetListRelatedBaiViet(int pageIndex, int pageSize, string baiVietSlug, string? keyword, string? danhMucSlug);
    }
}
