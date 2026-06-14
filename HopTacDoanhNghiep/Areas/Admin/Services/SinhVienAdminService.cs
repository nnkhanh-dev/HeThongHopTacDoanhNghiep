using HopTacDoanhNghiep.Areas.Admin.ViewModels.SinhVien;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.EntityFrameworkCore;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public class SinhVienAdminService : ISinhVienAdmin
    {
        private readonly AppDbContext _context;

        public SinhVienAdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PageResult<SinhVienVM>> GetListSinhVien(int pageIndex, int pageSize, string? keyword)
        {
            var query = _context.SinhViens
                                .AsNoTracking()
                                .Include(sv => sv.NguoiDung)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword)) {
                query = query.Where(sv => sv.MaSV.Contains(keyword) || sv.NguoiDung.HoTen.Contains(keyword));
            }

            var totalRecords = await query.CountAsync();

            var sinhViens = await query
                                .OrderBy(sv => sv.MaSV)
                                .Skip((pageIndex - 1) * pageSize)
                                .Take(pageSize)
                                .Select(sv => new SinhVienVM
                                {
                                    HoTen = sv.NguoiDung.HoTen,
                                    MaSV = sv.MaSV,
                                    EmailGiaoDuc = sv.EmailGiaoDuc,
                                    MaNguoiDung = sv.MaNguoiDung,
                                    HoSoNangLuc = sv.HoSoNangLuc,
                                    AnhThe = sv.AnhThe,
                                    GhiChu = sv.GhiChu
                                })
                                .ToListAsync();

            return new PageResult<SinhVienVM>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Records = sinhViens
            };
        }

        public async Task<BaseResult<SinhVienVM>> GetSinhVienByMaSV(string maSV)
        {
            if(string.IsNullOrEmpty(maSV))
            {
                return BaseResult<SinhVienVM>.Fail("Mã sinh viên không được để trống");
            }

            var sinhVien = await _context.SinhViens
                                        .AsNoTracking()
                                        .Include(sv => sv.NguoiDung)
                                        .FirstOrDefaultAsync(sv => sv.MaSV == maSV);

            if (sinhVien == null)
            {
                return BaseResult<SinhVienVM>.Fail("Sinh viên không tồn tại");
            }

            var sinhVienVM = new SinhVienVM
            {
                HoTen = sinhVien.NguoiDung.HoTen,
                MaSV = sinhVien.MaSV,
                EmailGiaoDuc = sinhVien.EmailGiaoDuc,
                MaNguoiDung = sinhVien.MaNguoiDung,
                HoSoNangLuc = sinhVien.HoSoNangLuc,
                AnhThe = sinhVien.AnhThe,
                GhiChu = sinhVien.GhiChu
            };

            return BaseResult<SinhVienVM>.Success(sinhVienVM);
        }
    }
}
