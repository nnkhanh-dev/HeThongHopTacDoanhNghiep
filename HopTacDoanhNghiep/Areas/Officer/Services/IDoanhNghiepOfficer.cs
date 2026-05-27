using HopTacDoanhNghiep.Areas.Officer.ViewModels;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Officer.Services
{
    public interface IDoanhNghiepOfficer
    {
        Task<PageResult<DangKyDoanhNghiepVM>> GetListDangKyDoanhNghiep(int pageIndex, int pageSize, string? keyword, string MaCB);
        Task<PageResult<DoanhNghiepVM>> GetListDoanhNghiep(int pageIndex, int pageSize, string keyword);
        Task<BaseResult<DoanhNghiepVM>> GetDoanhNghiepByMaDN(string MaDN);
    }
}
