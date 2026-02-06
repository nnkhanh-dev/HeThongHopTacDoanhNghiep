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

        public async Task<PageResult<SinhVienVM>> GetListSinhVien(int pageIndex = 1, int pageSize = 10, string? keyword = null, string? khoa = null, string? chuyenNganh = null)
        {
            var query = _context.SinhViens.AsNoTracking();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(sv => sv.HoTen.Contains(keyword) || sv.MaSV.Contains(keyword) || sv.Khoa.Contains(keyword) || sv.ChuyenNganh.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(khoa))
            {
                query = query.Where(sv => sv.Khoa == khoa);
            }

            if (!string.IsNullOrEmpty(chuyenNganh))
            {
                query = query.Where(sv => sv.ChuyenNganh == chuyenNganh);
            }

            var totalRecords = await query.CountAsync();

            if (pageIndex < 1) pageIndex = 1;
            if (pageSize <= 0) pageSize = 10;

            var sinhViens = await query
                .OrderBy(sv => sv.HoTen)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(sv => new SinhVienVM
                {
                    Id = sv.Id,
                    HoTen = sv.HoTen,
                    MaSV = sv.MaSV,
                    Lop = sv.Lop,
                    Khoa = sv.Khoa,
                    ChuyenNganh = sv.ChuyenNganh
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

        public async Task<BaseResult<SinhVienVM>> GetSinhVienById(Guid id)
        {
            var sinhVien = await _context.SinhViens.AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new SinhVienVM
                {
                    Id = x.Id,
                    HoTen = x.HoTen,
                    MaSV = x.MaSV,
                    NgaySinh = x.NgaySinh,
                    Lop = x.Lop,
                    Khoa = x.Khoa,
                    Email = x.Email,
                    SDT = x.SDT,
                    ChuyenNganh = x.ChuyenNganh,
                    AnhThe = x.AnhThe,
                    TimViec = x.TimViec,
                    GhiChu = x.GhiChu
                })
                .FirstOrDefaultAsync();

            if (sinhVien == null)
            {
                return BaseResult<SinhVienVM>.Fail("Sinh viên không tồn tại");
            }

            return BaseResult<SinhVienVM>.Success(sinhVien, "Lấy thông tin sinh viên thành công!");
        }
    }
}
