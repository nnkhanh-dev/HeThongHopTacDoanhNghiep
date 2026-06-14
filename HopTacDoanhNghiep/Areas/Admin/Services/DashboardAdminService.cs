using HopTacDoanhNghiep.Areas.Admin.ViewModels.Dashboard;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.Enums.HopTac;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.EntityFrameworkCore;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public class DashboardAdminService : IDashboardAdmin
    {
        private readonly AppDbContext _context;

        public DashboardAdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResult<DashboardDataVM>> GetDashboardData()
        {
            var canBos = await _context.CanBos
                                        .Include(x => x.NguoiDung)
                                        .CountAsync(x => x.NguoiDung.TrangThai == Enums.NguoiDung.NguoiDungStatus.HoatDong);

            var doanhNghieps = await _context.DoanhNghieps.CountAsync(x => x.TrangThaiHopTac == HopTacDoanhNghiepStatus.DuyetHopTac);

            var sinhViens = await _context.SinhViens
                                            .Include(x => x.NguoiDung)
                                            .CountAsync(x => x.NguoiDung.TrangThai == Enums.NguoiDung.NguoiDungStatus.HoatDong);

            var tinTuyenDung = await _context.TinTuyenDungs.CountAsync(x => x.Status == Enums.ViecLam.ViecLamStatus.CongBo);

            var dashboardData = new DashboardDataVM
            {
                CanBos = canBos,
                DoanhNghieps = doanhNghieps,
                SinhViens = sinhViens,
                TinTuyenDungs = tinTuyenDung
            };

            return BaseResult<DashboardDataVM>.Success(dashboardData);
        }
    }
}
