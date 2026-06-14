using HopTacDoanhNghiep.Areas.Admin.ViewModels.TinTuyenDung;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.Enums.ViecLam;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.EntityFrameworkCore;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public class TinTuyenDungAdminService : ITinTuyenDungAdmin
    {
        private readonly AppDbContext _context;

        public TinTuyenDungAdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PageResult<TinTuyenDungVM>> GetListTinTuyenDung(int pageIndex, int pageSize, string? keyword)
        {
            var query = _context.TinTuyenDungs
                                .AsNoTracking()
                                .Include(x => x.DoanhNghiep)
                                .Where(x => x.DeletedAt == null && x.Status == ViecLamStatus.CongBo);
                                

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.TieuDe.Contains(keyword) || x.DoanhNghiep.TenHienThi.Contains(keyword));
            }

            var totalRecords = await query.CountAsync();

            var records = await query.OrderByDescending(x => x.CreatedAt)
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
                                         MaDoanhNghiep = x.DoanhNghiep.MaDN,
                                         DoanhNghiep = x.DoanhNghiep.TenHienThi,
                                         LogoDoanhNghiep = x.DoanhNghiep.Logo,
                                         CreatedAt = x.CreatedAt
                                     })
                                     .ToListAsync();

            return new PageResult<TinTuyenDungVM>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Records = records
            };
        }

        public async Task<BaseResult<TinTuyenDungVM>> GetTinTuyenDungByMaTTD(int MaTTD)
        {
            if(MaTTD <= 0 )
            {
                return BaseResult<TinTuyenDungVM>.Fail("MaTTD không hợp lệ");
            }

            var tinTuyenDung = await _context.TinTuyenDungs
                                            .AsNoTracking()
                                            .Where(t => t.MaTTD == MaTTD)
                                            .Include(x => x.DoanhNghiep)
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
                                                MaDoanhNghiep = x.DoanhNghiep.MaDN,
                                                DoanhNghiep = x.DoanhNghiep.TenHienThi,
                                                LogoDoanhNghiep = x.DoanhNghiep.Logo,
                                                CreatedAt = x.CreatedAt
                                            })
                                            .FirstOrDefaultAsync();

            if(tinTuyenDung == null)
            {
                return BaseResult<TinTuyenDungVM>.Fail("Không tìm thấy tin tuyển dụng");
            }

            return BaseResult<TinTuyenDungVM>.Success(tinTuyenDung);
        }
    }
}
