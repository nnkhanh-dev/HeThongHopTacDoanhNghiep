using DocumentFormat.OpenXml.Spreadsheet;
using HopTacDoanhNghiep.Areas.Admin.ViewModels.CanBo;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.Models;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public class CanBoAdminService : ICanBoAdmin
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CanBoAdminService(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<BaseResult> CreateCanBo(CreateCanBoVM canBo, string createdBy)
        {
            // check if email already exists
            var exists = await _userManager.FindByEmailAsync(canBo.Email);
            if (exists != null)
            {
                return BaseResult.Fail("Email đã tồn tại");
            }

            var user = new AppUser
            {
                UserName = await GenerateMaCB(),
                Email = canBo.Email,
                EmailConfirmed = true,
                HoTen = canBo.HoTen,
                PhoneNumber = canBo.SDT,
                TrangThai = canBo.TrangThai
            };

            var password = "CanBo@123";
            var createUserResult = await _userManager.CreateAsync(user, password);
            if (!createUserResult.Succeeded)
            {
                var errors = string.Join("; ", createUserResult.Errors.Select(e => e.Description));
                return BaseResult.Fail($"Không thể tạo người dùng: {errors}");
            }

            // add to Officer role if exists
            try
            {
                await _userManager.AddToRoleAsync(user, "Officer");
            }
            catch { }

            var entity = new CanBo
            {
                MaCB = user.UserName,
                MaChucVu = canBo.MaCV ?? 0,
                MaDonVi = canBo.MaDV ?? 0,
                MaNguoiDung = user.Id,
                BHTT = canBo.BHTT,
                BHTN = canBo.BHTN,
                STK = canBo.STK,
                AnhThe = canBo.AnhThe,
                GhiChu = null,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = createdBy
            };

            _context.CanBos.Add(entity);
            await _context.SaveChangesAsync();

            return BaseResult.Success();
        }

        private async Task<string> GenerateMaCB()
        {
            var now = DateTime.Now;

            var day = now.Day.ToString("D2");
            var month = now.Month.ToString("D2");
            var year = now.Year.ToString();

            var start = now.Date;
            var end = start.AddDays(1);

            var numberOfCanBoToday = await _context.CanBos
                .CountAsync(x => x.CreatedAt >= start && x.CreatedAt < end);

            var prefix = "CB";

            var suffix = (1000 + numberOfCanBoToday + 1).ToString();

            return $"{prefix}{year}{month}{day}{suffix}";
        }

        public async Task<BaseResult> EditCanBo(string maCanBo, EditCanBoVM canBo, string updatedBy)
        {
            var entity = await _context.CanBos.Include(x => x.NguoiDung).FirstOrDefaultAsync(x => x.MaCB == maCanBo && x.DeletedAt == null);
            if (entity == null)
                return BaseResult.Fail("Cán bộ không tìm thấy");

            // update related user
            var user = entity.NguoiDung;
            if (user != null)
            {
                user.HoTen = canBo.HoTen;
                user.Email = canBo.Email;
                user.UserName = canBo.Email;
                user.PhoneNumber = canBo.SDT;
                user.TrangThai = canBo.TrangThai;

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    var errors = string.Join("; ", updateResult.Errors.Select(e => e.Description));
                    return BaseResult.Fail($"Không thể cập nhật người dùng: {errors}");
                }
            }

            entity.MaChucVu = canBo.MaCV ?? 0;
            entity.MaDonVi = canBo.MaDV ?? 0;
            entity.BHTT = canBo.BHTT;
            entity.BHTN = canBo.BHTN;
            entity.STK = canBo.STK;
            entity.AnhThe = canBo.AnhThe;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = updatedBy;

            await _context.SaveChangesAsync();

            return BaseResult.Success();
        }

        public async Task<PageResult<CanBoVM>> GetListCanBo(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var query = _context.CanBos.AsNoTracking().Include(x => x.NguoiDung).Where(x => x.DeletedAt == null);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.MaCB.Contains(keyword) || x.NguoiDung.HoTen.Contains(keyword) || x.NguoiDung.Email.Contains(keyword) || x.NguoiDung.PhoneNumber.Contains(keyword));
            }

            var totalRecords = await query.CountAsync();

            if (pageIndex < 1) pageIndex = 1;
            if (pageSize <= 0) pageSize = 10;

            var records = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new CanBoVM
                {
                    MaCB = x.MaCB,
                    MaDV = x.MaDonVi,
                    TenDonVi = x.DonVi.TenDV,
                    MaCV = x.MaChucVu,
                    TenChucVu = x.ChucVu.TenChucVu,
                    BHTT = x.BHTT,
                    BHTN = x.BHTN,
                    STK = x.STK,
                    AnhThe = x.AnhThe,
                    HoTen = x.NguoiDung.HoTen,
                    SDT = x.NguoiDung.PhoneNumber,
                    Email = x.NguoiDung.Email,
                    TrangThai = x.NguoiDung.TrangThai
                })
                .ToListAsync();

            return new PageResult<CanBoVM>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Records = records
            };
        }

        public async Task<BaseResult<CanBoVM>> GetCanBoByMaCB(string maCanBo)
        {
            var result = await _context.CanBos
                .AsNoTracking()
                .Include(x => x.NguoiDung)
                .Where(x => x.MaCB == maCanBo && x.DeletedAt == null)
                .Select(x => new CanBoVM
                {
                    MaCB = x.MaCB,
                    MaDV = x.MaDonVi,
                    TenDonVi = x.DonVi.TenDV,
                    MaCV = x.MaChucVu,
                    TenChucVu = x.ChucVu.TenChucVu,
                    BHTT = x.BHTT,
                    BHTN = x.BHTN,
                    STK = x.STK,
                    AnhThe = x.AnhThe,
                    HoTen = x.NguoiDung.HoTen,
                    SDT = x.NguoiDung.PhoneNumber,
                    Email = x.NguoiDung.Email,
                    TrangThai = x.NguoiDung.TrangThai
                })
                .FirstOrDefaultAsync();

            if (result == null)
            {
                return BaseResult<CanBoVM>.Fail("Cán bộ không tồn tại.");
            }

            return BaseResult<CanBoVM>.Success(result, "Lấy thông tin cán bộ thành công.");
        }
    }
}
