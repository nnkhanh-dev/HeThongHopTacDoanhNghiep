using HopTacDoanhNghiep.Areas.Company.ViewModels.ViecLam;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.Enums.ViecLam;
using HopTacDoanhNghiep.Models;
using HopTacDoanhNghiep.Services;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.EntityFrameworkCore;

namespace HopTacDoanhNghiep.Areas.Company.Services
{
    public class TinTuyenDungCompanyService : ITinTuyenDungCompany
    {
        private readonly AppDbContext _context;
        private readonly ISlug _slugService;

        public TinTuyenDungCompanyService(AppDbContext context, ISlug slugService)
        {
            _context = context;
            _slugService = slugService;
        }

        public async Task<PageResult<TinTuyenDungVM>> GetListTinTuyenDung(
            string MaDoanhNghiep,
            int pageIndex,
            int pageSize,
            string? keyword = null,
            ViecLamStatus? status = null,
            ViecLamType? loaiViecLam = null,
            DoiTuongUngTuyen? doiTuongUngTuyen = null,
            TrinhDoType? trinhDo = null,
            long? luongMin = null,
            long? luongMax = null,
            bool? conHieuLuc = null,
            bool? sapXepLuongToiDa = null,
            string? sapXepTheo = null
        )
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.TinTuyenDungs
                .AsNoTracking()
                .Where(x => x.DeletedAt == null && x.MaDoanhNgiep.ToString() == MaDoanhNghiep);

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



        public async Task<BaseResult<TinTuyenDungVM>> GetTinTuyenDungById(int maTTD)
        {
            var item = await _context.TinTuyenDungs
                .AsNoTracking()
                .Where(x => x.MaTTD == maTTD && x.DeletedAt == null)
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

        public async Task<BaseResult> CreateTinTuyenDung(TinTuyenDungCreateVM tinTuyenDung)
        {
            if (tinTuyenDung == null)
                return BaseResult.Fail("Dữ liệu việc làm không hợp lệ");

            // Check doanh nghiệp 
            var doanhNghiep = await _context.DoanhNghieps.AsNoTracking().Where(x => x.MaDN.ToString() == tinTuyenDung.MaDoanhNghiep && x.DeletedAt == null).FirstOrDefaultAsync();
            if (doanhNghiep == null)
                return BaseResult.Fail("Không tìm thấy thông tin doanh nghiệp");

            try
            {
                // Validate dates (nullable-safe)
                if (tinTuyenDung.NgayBatDau.HasValue && tinTuyenDung.NgayHetHan.HasValue &&
                    tinTuyenDung.NgayHetHan <= tinTuyenDung.NgayBatDau)
                {
                    return BaseResult.Fail("Ngày hết hạn phải sau ngày bắt đầu");
                }

                // Validate salary (nullable-safe)
                if (tinTuyenDung.LuongToiThieu.HasValue && tinTuyenDung.LuongToiDa.HasValue &&
                    tinTuyenDung.LuongToiDa < tinTuyenDung.LuongToiThieu)
                {
                    return BaseResult.Fail("Lương tối đa phải lớn hơn hoặc bằng lương tối thiểu");
                }

                // Generate unique slug
                var slug = await _slugService.GenerateUniqueSlugAsync(
                    tinTuyenDung.TieuDe,
                    _context.TinTuyenDungs.AsNoTracking(),
                    x => x.Slug
                );

                var now = DateTime.UtcNow;

                var entity = new TinTuyenDung
                {
                    TieuDe = tinTuyenDung.TieuDe,
                    Slug = slug,
                    MoTa = tinTuyenDung.MoTa,
                    YeuCau = tinTuyenDung.YeuCau,
                    UuTien = tinTuyenDung.UuTien,
                    QuyenLoi = tinTuyenDung.QuyenLoi,
                    DiaDiem = tinTuyenDung.DiaDiem,
                    TuKhoa = tinTuyenDung.TuKhoa,

                    // Chỉ gán khi có giá trị, tránh đổi nghĩa nghiệp vụ
                    LuongToiThieu = tinTuyenDung.LuongToiThieu ?? 0,
                    LuongToiDa = tinTuyenDung.LuongToiDa ?? 0,

                    NgayBatDau = tinTuyenDung.NgayBatDau ?? now,
                    NgayHetHan = tinTuyenDung.NgayHetHan ?? now.AddMonths(1),

                    LoaiViecLam = tinTuyenDung.LoaiViecLam ?? ViecLamType.ThucTap,
                    DoiTuongUngTuyen = tinTuyenDung.DoiTuongUngTuyen ?? DoiTuongUngTuyen.ThucTapSinh,
                    TrinhDo = tinTuyenDung.TrinhDo ?? TrinhDoType.DaiHoc,
                    Status = tinTuyenDung.Status ?? ViecLamStatus.Nhap,

                    MaDoanhNgiep = doanhNghiep.MaDN,
                    CreatedAt = now,
                    CreatedBy = tinTuyenDung.CreatedBy
                };

                _context.TinTuyenDungs.Add(entity);
                await _context.SaveChangesAsync();

                return BaseResult.Success("Tạo việc làm thành công");
            }
            catch (Exception ex)
            {
                return BaseResult.Fail("Tạo việc làm thất bại");
            }
        } // done

        public async Task<BaseResult> EditTinTuyenDung(int id, TinTuyenDungEditVM model)
        {
            if (model == null)
                return BaseResult.Fail("Dữ liệu tin tuyển dụng không hợp lệ");

            var viecLam = await _context.TinTuyenDungs
                .FirstOrDefaultAsync(x => x.MaTTD == id && x.DeletedAt == null);

            if (viecLam == null)
                return BaseResult.Fail("Việc làm không tồn tại");

            if (model.UpdatedBy != viecLam.MaDoanhNgiep.ToString())
            {
                return BaseResult.Fail("Không có quyền chỉnh sửa");
            }

            try
            {
                // Validate dates (xử lý nullable)
                if (model.NgayBatDau.HasValue && model.NgayHetHan.HasValue &&
                    model.NgayHetHan <= model.NgayBatDau)
                {
                    return BaseResult.Fail("Ngày hết hạn phải sau ngày bắt đầu");
                }

                // Validate salary (xử lý nullable)
                if (model.LuongToiThieu.HasValue && model.LuongToiDa.HasValue &&
                    model.LuongToiDa < model.LuongToiThieu)
                {
                    return BaseResult.Fail("Lương tối đa phải lớn hơn hoặc bằng lương tối thiểu");
                }

                // Kiểm tra và cập nhật slug nếu tiêu đề thay đổi
                if (!string.Equals(model.TieuDe, viecLam.TieuDe, StringComparison.Ordinal))
                {
                    var slug = await _slugService.GenerateUniqueSlugAsync(
                        model.TieuDe,
                        _context.TinTuyenDungs.AsNoTracking().Where(x => x.MaTTD != id),
                        x => x.Slug
                    );

                    viecLam.TieuDe = model.TieuDe;
                    viecLam.Slug = slug;
                }

                // Update các field đơn giản
                viecLam.MoTa = model.MoTa;
                viecLam.YeuCau = model.YeuCau;
                viecLam.UuTien = model.UuTien;
                viecLam.QuyenLoi = model.QuyenLoi;
                viecLam.DiaDiem = model.DiaDiem;
                viecLam.TuKhoa = model.TuKhoa;

                if (model.LuongToiThieu.HasValue)
                    viecLam.LuongToiThieu = model.LuongToiThieu.Value;

                if (model.LuongToiDa.HasValue)
                    viecLam.LuongToiDa = model.LuongToiDa.Value;

                viecLam.NgayBatDau = model.NgayBatDau ?? viecLam.NgayBatDau;
                viecLam.NgayHetHan = model.NgayHetHan ?? viecLam.NgayHetHan;
                viecLam.LoaiViecLam = model.LoaiViecLam ?? viecLam.LoaiViecLam;
                viecLam.DoiTuongUngTuyen = model.DoiTuongUngTuyen ?? viecLam.DoiTuongUngTuyen;
                viecLam.TrinhDo = model.TrinhDo ?? viecLam.TrinhDo;
                viecLam.Status = model.Status ?? viecLam.Status;

                viecLam.UpdatedAt = DateTime.UtcNow;
                viecLam.UpdatedBy = model.UpdatedBy;

                await _context.SaveChangesAsync();

                return BaseResult.Success("Cập nhật việc làm thành công");
            }
            catch (Exception ex)
            {

                return BaseResult.Fail("Cập nhật việc làm thất bại");
            }
        } // done

        public async Task<BaseResult> DeleteTinTuyenDung(int id, string deletedBy)
        {
            var viecLam = await _context.TinTuyenDungs
                .FirstOrDefaultAsync(x => x.MaTTD == id && x.DeletedAt == null);

            if (viecLam == null)
                return BaseResult.Fail("Việc làm không tồn tại");

            if (deletedBy != viecLam.MaDoanhNgiep.ToString())
            {
                return BaseResult.Fail("Không có quyền xóa");
            }

            try
            {
                viecLam.DeletedAt = DateTime.UtcNow;
                viecLam.DeletedBy = deletedBy;

                await _context.SaveChangesAsync();

                return BaseResult.Success("Xóa việc làm thành công");
            }
            catch (Exception ex)
            {
                return BaseResult.Fail("Xóa việc làm thất bại");
            }
        }
    }
}
