using HopTacDoanhNghiep.Areas.Company.ViewModels.LinhVuc;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Company.Services
{
    public interface ILinhVucCompany
    {
        Task<PageResult<LinhVucVM>> GetListLinhVuc(int pageIndex = 1, int pageSize = 10, string? keyword = null);
    }
}
