using HopTacDoanhNghiep.Areas.Admin.ViewModels.TinTuyenDung;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public interface ITinTuyenDungAdmin
    {
        Task<PageResult<TinTuyenDungVM>> GetListTinTuyenDung(int pageIndex, int pageSize, string? keyword);
        Task<BaseResult<TinTuyenDungVM>> GetTinTuyenDungByMaTTD(int MaTTD);
    }
}
