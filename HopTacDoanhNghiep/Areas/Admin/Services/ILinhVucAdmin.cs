using HopTacDoanhNghiep.Areas.Admin.ViewModels.LinhVuc;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public interface ILinhVucAdmin
    {
        Task<PageResult<LinhVucVM>> GetListLinhVuc (int pageIndex = 1, int pageSize = 10, string? keyword = null);
        Task<BaseResult<LinhVucVM>> GetLinhVucById (int id);
        Task<BaseResult> CreateLinhVuc (LinhVucCreateVM model);
        Task<BaseResult> EditLinhVuc(int id, LinhVucEditVM model);
        Task<BaseResult> DeleteLinhVuc (int id, string deletedBy);
    }
}
