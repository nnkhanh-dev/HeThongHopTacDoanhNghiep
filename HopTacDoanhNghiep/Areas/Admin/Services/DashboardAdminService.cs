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

        public async Task<BaseResult<DashboardDataVM>> GetDashboardData(int? year)
        {
            if (!year.HasValue)
            {
                year = DateTime.Now.Year;
            }

            var canBos = await _context.CanBos.AsNoTracking()
                                        .Include(x => x.NguoiDung)
                                        .CountAsync(x => x.NguoiDung.TrangThai == Enums.NguoiDung.NguoiDungStatus.HoatDong && x.CreatedAt.Year == year);

            var doanhNghieps = await _context.DoanhNghieps.CountAsync(x => x.TrangThaiHopTac == HopTacDoanhNghiepStatus.DuyetHopTac && x.CreatedAt.Year == year);

            var sinhViens = await _context.SinhViens.AsNoTracking()
                                            .Include(x => x.NguoiDung)
                                            .CountAsync(x => x.NguoiDung.TrangThai == Enums.NguoiDung.NguoiDungStatus.HoatDong && x.CreatedAt.Year == year);    
            var tinTuyenDung = await _context.TinTuyenDungs.CountAsync(x => x.Status == Enums.ViecLam.ViecLamStatus.CongBo && x.CreatedAt.Year == year);

            var tinTuyenDungByMonth = await _context.TinTuyenDungs.AsNoTracking()
                                        .Where(x => x.Status == Enums.ViecLam.ViecLamStatus.CongBo && x.CreatedAt.Year == year)
                                        .GroupBy(x => x.CreatedAt.Month)
                                        .Select(x => new DashboardDataByLabelVM
                                        {
                                            Label = x.Key.ToString(),
                                            Count = x.Count()
                                        })
                                        .ToListAsync();

            var doanhNghiepByMonth = await _context.DoanhNghieps.AsNoTracking()
                                        .Where(x => x.TrangThaiHopTac == HopTacDoanhNghiepStatus.DuyetHopTac && x.CreatedAt.Year == year)
                                        .GroupBy(x => x.CreatedAt.Month)
                                        .Select(x => new DashboardDataByLabelVM
                                        {
                                            Label = x.Key.ToString(),
                                            Count = x.Count()
                                        })
                                        .ToListAsync();

            var tintuyenDungByDoanhNghiep = await _context.TinTuyenDungs.AsNoTracking()
                .Include(x => x.DoanhNghiep)
                .Where(x => x.Status == Enums.ViecLam.ViecLamStatus.CongBo && x.CreatedAt.Year == year && x.DeletedAt == null)
                .GroupBy(x => x.DoanhNghiep.TenHienThi)
               .Select(x => new DashboardDataByLabelVM
               {
                   Label = x.Key,
                   Count = x.Count()
               })
                                        .ToListAsync();

            var donUngTuyenByMonth = await _context.DonUngTuyens.AsNoTracking()
                                            .Where(x => x.DeletedAt == null && x.CreatedAt.Year == year)
                                            .GroupBy(x => x.CreatedAt.Month)
                                            .Select(x => new DashboardDataByLabelVM
                                            {
                                                Label = x.Key.ToString(),
                                                Count = x.Count()
                                            })
                                            .ToListAsync();

            var dashboardData = new DashboardDataVM
            {
                CanBos = canBos,
                DoanhNghieps = doanhNghieps,
                SinhViens = sinhViens,
                TinTuyenDungs = tinTuyenDung,
                TinTuyenDungByMonth = tinTuyenDungByMonth,
                DoanhNghiepByMonth = doanhNghiepByMonth,
                TintuyenDungByDoanhNghiep = tintuyenDungByDoanhNghiep,
                DonUngTuyenByMonth = donUngTuyenByMonth,

            };

            return BaseResult<DashboardDataVM>.Success(dashboardData);
        }
    }
}
