using HopTacDoanhNghiep.Areas.Admin.ViewModels.LinhVuc;
using HopTacDoanhNghiep.Areas.Admin.ViewModels.Nganh;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public interface INganhAdmin
    {
        Task<PageResult<NganhVM>> GetListNganh(int pageIndex = 1, int pageSize = 10, string? keyword = null);
        Task<BaseResult<NganhVM>> GetNganhById(int id);
        Task<BaseResult> CreateNganh(NganhCreateVM model);
        Task<BaseResult> EditNganh(int id, NganhEditVM model);
        Task<BaseResult> DeleteNganh(int id, string deletedBy);
    }
}
