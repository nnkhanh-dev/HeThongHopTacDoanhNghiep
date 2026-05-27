using System.Globalization;
using System.Text;
using HopTacDoanhNghiep.Areas.Officer.ViewModels;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.Enums.HopTac;
using HopTacDoanhNghiep.ViewModels.Common;
using HopTacDoanhNghiep.ViewModels.DonVi;
using Microsoft.EntityFrameworkCore;

namespace HopTacDoanhNghiep.Areas.Officer.Services
{
    public class DoanhNghiepOfficerService : IDoanhNghiepOfficer
    {
        private readonly AppDbContext _context;

        public DoanhNghiepOfficerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PageResult<DangKyDoanhNghiepVM>> GetListDangKyDoanhNghiep(int pageIndex, int pageSize, string? keyword, string MaCB)
        {
            if (pageIndex < 1)
                pageIndex = 1;

            if (pageSize <= 0)
                pageSize = 10;

            // Lấy thông tin cán bộ
            var canBo = await _context.CanBos.Include(x => x.DonVi).AsNoTracking().FirstOrDefaultAsync(x => x.MaCB == MaCB);

            var isPCTSV = canBo.DonVi.TenDV == "Phòng Công Tác Sinh Viên";
            var isBGH = canBo.DonVi.TenDV == "Ban Giám Hiệu";
            var isKhoa = canBo.DonVi.NhanDoiTac == true;

            var query = _context.DoanhNghieps.AsNoTracking();

            if(isBGH)
            {
                query = query.Where(x => x.TrangThaiHopTac == HopTacDoanhNghiepStatus.XacNhanHopTac);
            }

            if (isPCTSV)
            {
                query = query.Where(x => x.TrangThaiHopTac == HopTacDoanhNghiepStatus.ChoXuLy);
            }

            if (isKhoa)
            {
                query = query.Where(x =>
                    x.TrangThaiHopTac == HopTacDoanhNghiepStatus.XacNhanDoanhNghiep &&
                    x.HopTacDonVis.Any(h => h.MaDV == canBo.DonVi.MaDV)
                );
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.TenPhapLy.Contains(keyword.Trim()) || x.MaSoThue.Contains(keyword.Trim()));
            }

            var totalRecords = await query.CountAsync();

            var records = await query.OrderByDescending(x => x.CreatedAt)
                                     .Skip((pageIndex - 1) * pageSize)
                                     .Take(pageSize)
                                     .Select(x => new DangKyDoanhNghiepVM
                                     {
                                         MaDN = x.MaDN,
                                         TenHienThi = x.TenHienThi,
                                         TenPhapLy = x.TenPhapLy,
                                         MaSoThue = x.MaSoThue,
                                         Website = x.Website,
                                         Hotline = x.Hotline,
                                         EmailCongTy = x.EmailCongTy,
                                         NoiDungHopTac = x.NoiDungHopTac,
                                         TrangThaiHopTac = x.TrangThaiHopTac,
                                         CreatedAt = x.CreatedAt,
                                         UpdatedAt = x.UpdatedAt
                                     }).ToListAsync();

            return new PageResult<DangKyDoanhNghiepVM>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Records = records
             };
        }

        public async Task<PageResult<DoanhNghiepVM>> GetListDoanhNghiep(int pageIndex, int pageSize, string keyword)
        {
            if (pageIndex < 1)
                pageIndex = 1;

            if (pageSize <= 0)
                pageSize = 10;

            var query = _context.DoanhNghieps.Where(x => x.TrangThaiHopTac == HopTacDoanhNghiepStatus.DuyetHopTac).AsNoTracking();

            if (!string.IsNullOrEmpty(keyword))
            {
                var k = keyword.Trim();
                query = query.Where(x => x.TenHienThi.Contains(k) || x.MaDN.Contains(k) || (x.MaSoThue != null && x.MaSoThue.Contains(k)));
            }

            var total = await query.CountAsync();

            var records = await query.OrderByDescending(x => x.CreatedAt)
                                     .Skip((pageIndex - 1) * pageSize)
                                     .Take(pageSize)
                                     .Select(x => new DoanhNghiepVM
                                     {
                                         MaDN = x.MaDN,
                                         TenHienThi = x.TenHienThi,
                                         TenPhapLy = x.TenPhapLy,
                                         MaSoThue = x.MaSoThue,
                                         Website = x.Website,
                                         Hotline = x.Hotline,
                                         EmailCongTy = x.EmailCongTy,
                                         DiaChi = x.DiaChi,
                                         GioiThieu = x.GioiThieu,
                                         QuyMoNhanSu = x.QuyMoNhanSu,
                                         NoiDungHopTac = x.NoiDungHopTac,
                                         HoTenNguoiDaiDien = x.NguoiDung != null ? x.NguoiDung.HoTen : null,
                                         SoDienThoaiNguoiDaiDien = x.NguoiDung != null ? x.NguoiDung.PhoneNumber : null,
                                         EmailNguoiDaiDien = x.NguoiDung != null ? x.NguoiDung.Email : null,
                                         AnhNguoiDaiDien = x.NguoiDung != null ? x.NguoiDung.AnhDaiDien : null,
                                         TrangThaiHopTac = x.TrangThaiHopTac,
                                         CreatedAt = x.CreatedAt,
                                         UpdatedAt = x.UpdatedAt
                                     }).ToListAsync();

            return new PageResult<DoanhNghiepVM>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = total,
                Records = records
            };
        }

        public async Task<BaseResult<DoanhNghiepVM>> GetDoanhNghiepByMaDN(string MaDN)
        {
            if (string.IsNullOrEmpty(MaDN))
                return BaseResult<DoanhNghiepVM>.Fail("Mã doanh nghiệp không hợp lệ");

            var item = await _context.DoanhNghieps
                                     .Include(x => x.NguoiDung)
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(x => x.MaDN == MaDN);

            if (item == null)
                return BaseResult<DoanhNghiepVM>.Fail("Không tìm thấy doanh nghiệp");

            var vm = new DoanhNghiepVM
            {
                MaDN = item.MaDN,
                TenHienThi = item.TenHienThi,
                TenPhapLy = item.TenPhapLy,
                MaSoThue = item.MaSoThue,
                Website = item.Website,
                Hotline = item.Hotline,
                EmailCongTy = item.EmailCongTy,
                DiaChi = item.DiaChi,
                GioiThieu = item.GioiThieu,
                QuyMoNhanSu = item.QuyMoNhanSu,
                NoiDungHopTac = item.NoiDungHopTac,
                HoTenNguoiDaiDien = item.NguoiDung != null ? item.NguoiDung.HoTen : null,
                SoDienThoaiNguoiDaiDien = item.NguoiDung != null ? item.NguoiDung.PhoneNumber : null,
                EmailNguoiDaiDien = item.NguoiDung != null ? item.NguoiDung.Email : null,
                AnhNguoiDaiDien = item.NguoiDung != null ? item.NguoiDung.AnhDaiDien : null,
                TrangThaiHopTac = item.TrangThaiHopTac,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt
            };

            return BaseResult<DoanhNghiepVM>.Success(vm);
        }
    }
}
