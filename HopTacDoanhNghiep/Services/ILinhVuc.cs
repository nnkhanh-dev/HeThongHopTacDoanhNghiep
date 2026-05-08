using HopTacDoanhNghiep.ViewModels.Common;
using HopTacDoanhNghiep.ViewModels.LinhVuc;

namespace HopTacDoanhNghiep.Services
{
    public interface ILinhVuc
    {
        Task<PageResult<LinhVucVM>> GetListLinhVuc(int pageIndex = 1, int pageSize = 10, string? keyword = null);
    }
}
