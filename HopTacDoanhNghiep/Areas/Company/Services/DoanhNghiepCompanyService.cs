using HopTacDoanhNghiep.Areas.Company.ViewModels;
using HopTacDoanhNghiep.Areas.Company.ViewModels.DoanhNghiep;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.Models;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.EntityFrameworkCore;

namespace HopTacDoanhNghiep.Areas.Company.Services
{
    public class DoanhNghiepCompanyService : IDoanhNghiepCompany
    {
        private readonly AppDbContext _context;

        public DoanhNghiepCompanyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResult<DoanhNghiepVM>> GetDoanhNghiepInfo(string MaDoanhNghiep)
        {
            if (string.IsNullOrWhiteSpace(MaDoanhNghiep))
            {
                return BaseResult<DoanhNghiepVM>.Fail("Mã doanh nghiệp không hợp lệ");
            }

            var doanhNghiep = await _context.DoanhNghieps
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MaDN == MaDoanhNghiep && x.DeletedAt == null);

            if (doanhNghiep == null)
            {
                return BaseResult<DoanhNghiepVM>.Fail("Không tìm thấy thông tin doanh nghiệp");
            }

            var data = new DoanhNghiepVM
            {
                MaDN = doanhNghiep.MaDN,
                TenHienThi = doanhNghiep.TenHienThi,
                Website = doanhNghiep.Website,
                MaSoThue = doanhNghiep.MaSoThue,
                TenPhapLy = doanhNghiep.TenPhapLy,
                Hotline = doanhNghiep.Hotline,
                EmailCongTy = doanhNghiep.EmailCongTy,
                Logo = doanhNghiep.Logo,
                DiaChi = doanhNghiep.DiaChi,
                GioiThieu = doanhNghiep.GioiThieu,
                QuyMoNhanSu = doanhNghiep.QuyMoNhanSu,
                NoiDungHopTac = doanhNghiep.NoiDungHopTac,
                TrangThaiHopTac = doanhNghiep.TrangThaiHopTac,
                GhiChu = doanhNghiep.GhiChu,
                CreatedAt = doanhNghiep.CreatedAt,
                UpdatedAt = doanhNghiep.UpdatedAt
            };

            return BaseResult<DoanhNghiepVM>.Success(data, "Lấy thông tin doanh nghiệp thành công");
        }

        public async Task<BaseResult<NguoiDaiDienVM>> GetNguoiDaiDienInfo(string MaDoanhNghiep)
        {
            if (string.IsNullOrWhiteSpace(MaDoanhNghiep))
            {
                return BaseResult<NguoiDaiDienVM>.Fail("Mã doanh nghiệp không hợp lệ");
            }

            var dn = await _context.DoanhNghieps
                .Include(x => x.NguoiDung)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MaDN == MaDoanhNghiep && x.DeletedAt == null);

            if (dn == null)
            {
                return BaseResult<NguoiDaiDienVM>.Fail("Không tìm thấy doanh nghiệp");
            }

            var user = dn.NguoiDung;

            var vm = new NguoiDaiDienVM
            {
                HoTen = user?.HoTen,
                SoDienThoai = user?.PhoneNumber,
                Email = user?.Email,
                AnhNguoiDaiDien = user?.AnhDaiDien
            };

            return BaseResult<NguoiDaiDienVM>.Success(vm, "Lấy thông tin người đại diện thành công");
        }

        public async Task<BaseResult> UpdateDoanhNghiepInfo(string MaDoanhNghiep, DoanhNghiepUpdateVM updateVM)
        {
            if (string.IsNullOrWhiteSpace(MaDoanhNghiep))
            {
                return BaseResult.Fail("Mã doanh nghiệp không hợp lệ");
            }

            if (updateVM == null)
            {
                return BaseResult.Fail("Dữ liệu cập nhật không hợp lệ");
            }

            var doanhNghiep = await _context.DoanhNghieps
                .FirstOrDefaultAsync(x => x.MaDN == MaDoanhNghiep && x.DeletedAt == null);

            if (doanhNghiep == null)
            {
                return BaseResult.Fail("Không tìm thấy thông tin doanh nghiệp");
            }

            doanhNghiep.TenHienThi = updateVM.TenHienThi.Trim();
            doanhNghiep.Website = string.IsNullOrWhiteSpace(updateVM.Website) ? null : updateVM.Website.Trim();
            doanhNghiep.MaSoThue = string.IsNullOrWhiteSpace(updateVM.MaSoThue) ? null : updateVM.MaSoThue.Trim();
            doanhNghiep.TenPhapLy = string.IsNullOrWhiteSpace(updateVM.TenPhapLy) ? null : updateVM.TenPhapLy.Trim();
            doanhNghiep.Hotline = string.IsNullOrWhiteSpace(updateVM.Hotline) ? null : updateVM.Hotline.Trim();
            doanhNghiep.EmailCongTy = updateVM.EmailCongTy.Trim();
            doanhNghiep.Logo = string.IsNullOrWhiteSpace(updateVM.Logo) ? doanhNghiep.Logo : updateVM.Logo.Trim();
            doanhNghiep.DiaChi = string.IsNullOrWhiteSpace(updateVM.DiaChi) ? null : updateVM.DiaChi.Trim();
            doanhNghiep.GioiThieu = string.IsNullOrWhiteSpace(updateVM.GioiThieu) ? null : updateVM.GioiThieu.Trim();
            doanhNghiep.QuyMoNhanSu = updateVM.QuyMoNhanSu;
            doanhNghiep.UpdatedAt = DateTime.UtcNow;
            doanhNghiep.UpdatedBy = string.IsNullOrWhiteSpace(updateVM.UpdatedBy) ? doanhNghiep.UpdatedBy : updateVM.UpdatedBy;

            await _context.SaveChangesAsync();

            return BaseResult.Success("Cập nhật thông tin doanh nghiệp thành công");
        }

        public async Task<BaseResult> UpdateNguoiDaiDienInfo(string MaDoanhNghiep, NguoiDaiDienUpdateVM updateVM)
        {
            if (string.IsNullOrWhiteSpace(MaDoanhNghiep))
            {
                return BaseResult.Fail("Mã doanh nghiệp không hợp lệ");
            }

            if (updateVM == null)
            {
                return BaseResult.Fail("Dữ liệu cập nhật không hợp lệ");
            }

            var doanhNghiep = await _context.DoanhNghieps
                .Include(x => x.NguoiDung)
                .FirstOrDefaultAsync(x => x.MaDN == MaDoanhNghiep && x.DeletedAt == null);

            if (doanhNghiep == null)
            {
                return BaseResult.Fail("Không tìm thấy doanh nghiệp");
            }

            var user = doanhNghiep.NguoiDung;
            if (user == null)
            {
                return BaseResult.Fail("Người đại diện chưa được gán");
            }

            user.HoTen = updateVM.HoTen.Trim();
            user.PhoneNumber = updateVM.SoDienThoai.Trim();
            user.Email = updateVM.Email.Trim();
            user.AnhDaiDien = string.IsNullOrWhiteSpace(updateVM.AnhNguoiDaiDien) ? user.AnhDaiDien : updateVM.AnhNguoiDaiDien.Trim();

            await _context.SaveChangesAsync();

            return BaseResult.Success("Cập nhật người đại diện thành công");
        }
    }
}
