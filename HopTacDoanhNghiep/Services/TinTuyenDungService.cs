using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.Enums.BaiViet;
using HopTacDoanhNghiep.Enums.ViecLam;
using HopTacDoanhNghiep.ViewModels.Common;
using HopTacDoanhNghiep.ViewModels.ViecLam;
using Microsoft.EntityFrameworkCore;

namespace HopTacDoanhNghiep.Services
{
    public class TinTuyenDungService : ITinTuyenDung
    {
        private readonly AppDbContext _context;

        public TinTuyenDungService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PageResult<ViecLamVM>> GetListTinTuyenDung(
            int pageIndex = 1,
            int pageSize = 10,
            string? keyword = null,
            ViecLamType? loaiViecLam = null,
            TrinhDoType? trinhDo = null,
            DoiTuongUngTuyen? doiTuongUngTuyen = null,
            double? luongMin = null,
            double? luongMax = null,
            bool? sapXepLuongToiDa = null,
            string? sapXepTheo = null)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.TinTuyenDungs
                .AsNoTracking()
                .Where(x => x.DeletedAt == null && x.Status == ViecLamStatus.CongBo);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x =>
                    x.TieuDe.Contains(keyword) ||
                    x.MoTa.Contains(keyword) ||
                    x.TuKhoa.Contains(keyword) ||
                    x.DiaDiem.Contains(keyword));
            }

            if (loaiViecLam.HasValue)
                query = query.Where(x => x.LoaiViecLam == loaiViecLam.Value);

            if (doiTuongUngTuyen.HasValue)
                query = query.Where(x => x.DoiTuongUngTuyen == doiTuongUngTuyen.Value);

            if (trinhDo.HasValue)
                query = query.Where(x => x.TrinhDo == trinhDo.Value);

            if (luongMin.HasValue)
                query = query.Where(x => x.LuongToiThieu >= (decimal)luongMin.Value);

            if (luongMax.HasValue)
                query = query.Where(x => x.LuongToiDa <= (decimal)luongMax.Value);

            var total = await query.CountAsync();

            if (sapXepLuongToiDa == true)
            {
                if (string.Equals(sapXepTheo, "asc", StringComparison.OrdinalIgnoreCase))
                    query = query.OrderBy(x => x.LuongToiDa);
                else
                    query = query.OrderByDescending(x => x.LuongToiDa);
            }
            else
            {
                query = query.OrderByDescending(x => x.CreatedAt);
            }

            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ViecLamVM
                {
                    TieuDe = x.TieuDe,
                    Slug = x.Slug,
                    MoTa = x.MoTa,
                    LuongToiThieu = x.LuongToiThieu,
                    LuongToiDa = x.LuongToiDa,
                    DiaDiem = x.DiaDiem,
                    TuKhoa = x.TuKhoa,
                    NgayBatDau = x.NgayBatDau,
                    NgayHetHan = x.NgayHetHan,
                    LoaiViecLam = x.LoaiViecLam,
                    DoiTuongUngTuyen = x.DoiTuongUngTuyen,
                    TrinhDo = x.TrinhDo,
                    DoanhNghiep = x.DoanhNghiep != null ? x.DoanhNghiep.TenHienThi : null,
                    DoanhNghiepLogo = x.DoanhNghiep != null ? x.DoanhNghiep.Logo : null,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .ToListAsync();

            return new PageResult<ViecLamVM>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = total,
                Records = items
            };
        }

        public async Task<PageResult<ViecLamVM>> GetRelatedTinTuyenDung(int pageIndex, int pageSize, string viecLamSlug, string? keyword)
        {
            var query = _context.TinTuyenDungs.AsNoTracking().Where(x => x.Slug != viecLamSlug && x.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x => x.TieuDe.Contains(keyword) || x.MoTa.Contains(keyword));
            }

            var totalRecords = await query.CountAsync();

            var list = await query.OrderByDescending(x => x.CreatedAt)
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .Select(x => new ViecLamVM
                                    {
                                        TieuDe = x.TieuDe,
                                        Slug = x.Slug,
                                        MoTa = x.MoTa,
                                        LuongToiThieu = x.LuongToiThieu,
                                        LuongToiDa = x.LuongToiDa,
                                        DiaDiem = x.DiaDiem,
                                        TuKhoa = x.TuKhoa,
                                        NgayBatDau = x.NgayBatDau,
                                        NgayHetHan = x.NgayHetHan,
                                        LoaiViecLam = x.LoaiViecLam,
                                        DoiTuongUngTuyen = x.DoiTuongUngTuyen,
                                        TrinhDo = x.TrinhDo,
                                        DoanhNghiep = x.DoanhNghiep != null ? x.DoanhNghiep.TenHienThi : null,
                                        DoanhNghiepLogo = x.DoanhNghiep != null ? x.DoanhNghiep.Logo : null,
                                        CreatedAt = x.CreatedAt,
                                        UpdatedAt = x.UpdatedAt
                                    })
                                    .ToListAsync();

            return new PageResult<ViecLamVM>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Records = list
            };
        }

        public async Task<BaseResult<ViecLamVM>> GetTinTuyenDungBySlug(string slug)
        {
            var item = await _context.TinTuyenDungs
                .AsNoTracking()
                .Where(x => x.Slug == slug && x.DeletedAt == null)
                .Select(x => new ViecLamVM
                {
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
                    DoanhNghiep = x.DoanhNghiep != null ? x.DoanhNghiep.TenHienThi : null,
                    DoanhNghiepLogo = x.DoanhNghiep != null ? x.DoanhNghiep.Logo : null,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (item == null)
                return BaseResult<ViecLamVM>.Fail("Việc làm không tồn tại");

            return BaseResult<ViecLamVM>.Success(item, "Lấy dữ liệu việc làm thành công");
        }
    }
}
