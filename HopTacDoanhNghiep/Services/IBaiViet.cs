using HopTacDoanhNghiep.ViewModels.BaiViet;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Services
{
    public interface IBaiViet
    {
        public Task<BaseResult<DanhMucBaiVietVM>> GetDanhMucBySlug(string slug);
        public Task<PageResult<BaiVietVM>> GetListBaiViet(int pageIndex, int pageSize, string? keyword, string? danhMucSlug);
        public Task<BaseResult<BaiVietVM>> GetBaiVietBySlug(string slug);
    }
}
