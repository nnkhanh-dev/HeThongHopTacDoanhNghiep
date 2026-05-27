using HopTacDoanhNghiep.Areas.Admin.ViewModels.CanBo;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public interface ICanBoAdmin
    {
        Task<PageResult<CanBoVM>> GetListCanBo(int pageIndex = 1, int pageSize = 10, string? keyword = null);
        Task<BaseResult<CanBoVM>> GetCanBoByMaCB(string maCanBo);
        Task<BaseResult> CreateCanBo(CreateCanBoVM canBo, string createdBy);
        Task<BaseResult> EditCanBo(string maCanBo, EditCanBoVM canBo, string updatedBy);
    }
}
