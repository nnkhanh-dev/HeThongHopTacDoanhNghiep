
using HopTacDoanhNghiep.Areas.Student.ViewModels.DoanhNghiep;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Student.Services
{
    public interface IDoanhNghiepStudent
    {
        Task<PageResult<DoanhNghiepVM>> GetListDoanhNghiep(int pageIndex, int pageSize, string? keyword);
        Task<BaseResult<DoanhNghiepVM>> GetDoanhNghiepByMaDN(string maDN);
    }
}
