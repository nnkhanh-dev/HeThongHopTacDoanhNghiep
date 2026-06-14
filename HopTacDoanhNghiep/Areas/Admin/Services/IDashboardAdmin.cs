using HopTacDoanhNghiep.Areas.Admin.ViewModels.Dashboard;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public interface IDashboardAdmin
    {
        Task<BaseResult<DashboardDataVM>> GetDashboardData();
    }
}
