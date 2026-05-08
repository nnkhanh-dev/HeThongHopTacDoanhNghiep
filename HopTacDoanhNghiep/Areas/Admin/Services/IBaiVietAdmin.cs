using HopTacDoanhNghiep.Areas.Admin.ViewModels.BaiViet;
using HopTacDoanhNghiep.Enums;
using HopTacDoanhNghiep.Enums.BaiViet;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public interface IBaiVietAdmin
    {
        Task<PageResult<BaiVietVM>> GetListBaiViet(int pageIndex, int pageSize, string? keyword = null, int? danhMucId = null, BaiVietStatus? status = null);
        Task<BaseResult<BaiVietVM>> GetBaiVietById(int id);
        Task<BaseResult> CreateBaiViet(BaiVietCreateVM baiViet);
        Task<BaseResult> EditBaiViet(int id, BaiVietEditVM baiViet);
        Task<BaseResult> DeleteBaiViet(int id, string deletedBy);
    }
}
