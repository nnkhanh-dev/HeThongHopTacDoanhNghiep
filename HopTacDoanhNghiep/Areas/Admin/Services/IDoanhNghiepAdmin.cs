using HopTacDoanhNghiep.Areas.Admin.ViewModels.DoanhNghiep;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public interface IDoanhNghiepAdmin
    {
        Task<PageResult<DoanhNghiepVM>> GetListDoanhNghiep(int pageIndex, int pageSize, string? keyword);
        Task<BaseResult<DoanhNghiepVM>> GetDoanhNghiepByMaDN (string maDN);
    }
}
