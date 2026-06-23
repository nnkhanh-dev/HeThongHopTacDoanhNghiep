using HopTacDoanhNghiep.Areas.Student.ViewModels.ViecLam;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.Enums.ViecLam;
using HopTacDoanhNghiep.Models;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.EntityFrameworkCore;

namespace HopTacDoanhNghiep.Areas.Student.Services
{
    public class TinTuyenDungStudentService : ITinTuyenDungStudent
    {
        private readonly AppDbContext _context;
        public TinTuyenDungStudentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PageResult<TinTuyenDungVM>> GetListTinTuyenDung(int pageIndex, int pageSize, string? keyword = null, ViecLamStatus? status = null, ViecLamType? loaiViecLam = null, DoiTuongUngTuyen? doiTuongUngTuyen = null, TrinhDoType? trinhDo = null, long? luongMin = null, long? luongMax = null, bool? conHieuLuc = null, bool? sapXepLuongToiDa = null, string? sapXepTheo = null)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.TinTuyenDungs
                .AsNoTracking()
                .Where(x => x.DeletedAt == null && x.Status == ViecLamStatus.CongBo);

            // ===== FILTER =====
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x =>
                    x.TieuDe.Contains(keyword) ||
                    x.MoTa.Contains(keyword) ||
                    x.TuKhoa.Contains(keyword) ||
                    x.DiaDiem.Contains(keyword));
            }

            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);

            if (loaiViecLam.HasValue)
                query = query.Where(x => x.LoaiViecLam == loaiViecLam.Value);

            if (doiTuongUngTuyen.HasValue)
                query = query.Where(x => x.DoiTuongUngTuyen == doiTuongUngTuyen.Value);

            if (trinhDo.HasValue)
                query = query.Where(x => x.TrinhDo == trinhDo.Value);

            if (luongMin.HasValue)
                query = query.Where(x => x.LuongToiThieu >= luongMin.Value);

            if (luongMax.HasValue)
                query = query.Where(x => x.LuongToiDa <= luongMax.Value);

            if (conHieuLuc.HasValue)
            {
                var now = DateTime.UtcNow;
                if (conHieuLuc.Value)
                    query = query.Where(x => x.NgayBatDau <= now && x.NgayHetHan >= now);
                else
                    query = query.Where(x => x.NgayBatDau > now || x.NgayHetHan < now);
            }

            // ===== COUNT =====
            var total = await query.CountAsync();

            // ===== SORT (CHỈ THEO LƯƠNG TỐI ĐA) =====
            if (sapXepLuongToiDa == true)
            {
                if (string.Equals(sapXepTheo, "asc", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderBy(x => x.LuongToiDa);
                }
                else
                {
                    // mặc định desc
                    query = query.OrderByDescending(x => x.LuongToiDa);
                }
            }
            else
            {
                // mặc định: mới nhất
                query = query.OrderByDescending(x => x.CreatedAt);
            }

            // ===== PAGING + SELECT =====
            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new TinTuyenDungVM
                {
                    MaTTD = x.MaTTD,
                    TieuDe = x.TieuDe,
                    Slug = x.Slug,
                    MoTa = x.MoTa,
                    YeuCau = x.YeuCau,
                    UuTien = x.UuTien,
                    QuyenLoi = x.QuyenLoi,
                    LuongToiThieu = x.LuongToiThieu,
                    LuongToiDa = x.LuongToiDa,
                    DiaDiem = x.DiaDiem,
                    TuKhoa = x.TuKhoa,
                    NgayBatDau = x.NgayBatDau,
                    NgayHetHan = x.NgayHetHan,
                    LoaiViecLam = x.LoaiViecLam,
                    DoiTuongUngTuyen = x.DoiTuongUngTuyen,
                    TrinhDo = x.TrinhDo,
                    Status = x.Status,
                    MaDoanhNghiep = x.MaDoanhNgiep,
                    DoanhNghiep = x.DoanhNghiep != null ? x.DoanhNghiep.TenHienThi : null,
                    LogoDoanhNghiep = x.DoanhNghiep != null ? x.DoanhNghiep.Logo : null,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();

            return new PageResult<TinTuyenDungVM>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = total,
                Records = items
            };
        }

        public async Task<PageResult<TinTuyenDungVM>> GetTinTuyenDungByCompanyId(
    string maDN,
    int pageIndex,
    int pageSize,
    string? keyword = null)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.TinTuyenDungs
                .AsNoTracking()
                .Where(x =>
                    x.DeletedAt == null &&
                    x.Status == ViecLamStatus.CongBo &&
                    x.MaDoanhNgiep == maDN);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x =>
                    x.TieuDe.Contains(keyword) ||
                    x.MoTa.Contains(keyword) ||
                    x.TuKhoa.Contains(keyword) ||
                    x.DiaDiem.Contains(keyword));
            }

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new TinTuyenDungVM
                {
                    MaTTD = x.MaTTD,
                    TieuDe = x.TieuDe,
                    Slug = x.Slug,
                    MoTa = x.MoTa,
                    YeuCau = x.YeuCau,
                    UuTien = x.UuTien,
                    QuyenLoi = x.QuyenLoi,
                    LuongToiThieu = x.LuongToiThieu,
                    LuongToiDa = x.LuongToiDa,
                    DiaDiem = x.DiaDiem,
                    TuKhoa = x.TuKhoa,
                    NgayBatDau = x.NgayBatDau,
                    NgayHetHan = x.NgayHetHan,
                    LoaiViecLam = x.LoaiViecLam,
                    DoiTuongUngTuyen = x.DoiTuongUngTuyen,
                    TrinhDo = x.TrinhDo,
                    Status = x.Status,
                    MaDoanhNghiep = x.MaDoanhNgiep,
                    DoanhNghiep = x.DoanhNghiep != null
                        ? x.DoanhNghiep.TenHienThi
                        : null,
                    LogoDoanhNghiep = x.DoanhNghiep != null
                        ? x.DoanhNghiep.Logo
                        : null,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();

            return new PageResult<TinTuyenDungVM>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = total,
                Records = items
            };
        }

        public async Task<BaseResult<TinTuyenDungVM>> GetTinTuyenDungBySlug(string slug)
        {
            var item = await _context.TinTuyenDungs
                .AsNoTracking()
                .Where(x => x.Slug == slug && x.DeletedAt == null)
                .Select(x => new TinTuyenDungVM
                {
                    MaTTD = x.MaTTD,
                    TieuDe = x.TieuDe,
                    Slug = x.Slug,
                    MoTa = x.MoTa,
                    YeuCau = x.YeuCau,
                    UuTien = x.UuTien,
                    QuyenLoi = x.QuyenLoi,
                    LuongToiThieu = x.LuongToiThieu,
                    LuongToiDa = x.LuongToiDa,
                    DiaDiem = x.DiaDiem,
                    TuKhoa = x.TuKhoa,
                    NgayBatDau = x.NgayBatDau,
                    NgayHetHan = x.NgayHetHan,
                    LoaiViecLam = x.LoaiViecLam,
                    DoiTuongUngTuyen = x.DoiTuongUngTuyen,
                    TrinhDo = x.TrinhDo,
                    Status = x.Status,
                    MaDoanhNghiep = x.MaDoanhNgiep,
                    DoanhNghiep = x.DoanhNghiep != null ? x.DoanhNghiep.TenHienThi : null,
                    CreatedAt = x.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (item == null)
            {
                return BaseResult<TinTuyenDungVM>.Fail("Việc làm không tồn tại");
            }

            return BaseResult<TinTuyenDungVM>.Success(item, "Lấy dữ liệu việc làm thành công");
        }
    }
}
