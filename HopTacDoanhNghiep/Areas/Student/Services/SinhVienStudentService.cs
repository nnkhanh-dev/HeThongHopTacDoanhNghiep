using HopTacDoanhNghiep.Areas.Student.ViewModels.SinhVien;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.EntityFrameworkCore;

namespace HopTacDoanhNghiep.Areas.Student.Services
{
    public class SinhVienStudentService : ISinhVienStudent
    {
        private readonly AppDbContext _context;

        public SinhVienStudentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResult<SinhVienVM>> GetStudentInfo(string maSinhVien)
        {
            if (string.IsNullOrWhiteSpace(maSinhVien))
            {
                return BaseResult<SinhVienVM>.Fail("Mã sinh viên không hợp lệ");
            }

            var student = await _context.SinhViens
                .AsNoTracking()
                .Include(x => x.NguoiDung)
                .Where(x => x.MaSV == maSinhVien && x.DeletedAt == null)
                .Select(x => new SinhVienVM
                {
                    MaSV = x.MaSV,
                    HoTen = x.NguoiDung != null ? x.NguoiDung.HoTen : string.Empty,
                    EmailGiaoDuc = x.EmailGiaoDuc,
                    Email = x.NguoiDung != null ? x.NguoiDung.Email : string.Empty,
                    SDT = x.NguoiDung != null ? x.NguoiDung.PhoneNumber : string.Empty,
                    Avatar = x.NguoiDung != null ? x.NguoiDung.AnhDaiDien : string.Empty,
                    HoSoNangLuc = x.HoSoNangLuc,
                    AnhThe = x.AnhThe
                })
                .FirstOrDefaultAsync();

            if (student == null)
            {
                return BaseResult<SinhVienVM>.Fail("Không tìm thấy thông tin sinh viên");
            }

            return BaseResult<SinhVienVM>.Success(student, "Lấy thông tin sinh viên thành công");
        }

        public async Task<BaseResult> EditStudentInfo(string maSinhVien, EditSinhVienVM editSinhVienVM)
        {
            if (string.IsNullOrWhiteSpace(maSinhVien))
            {
                return BaseResult.Fail("Mã sinh viên không hợp lệ");
            }

            if (editSinhVienVM == null)
            {
                return BaseResult.Fail("Dữ liệu cập nhật không hợp lệ");
            }

            var student = await _context.SinhViens
                .Include(x => x.NguoiDung)
                .Where(x => x.MaSV == maSinhVien && x.DeletedAt == null)
                .FirstOrDefaultAsync();

            if (student == null)
            {
                return BaseResult.Fail("Không tìm thấy thông tin sinh viên");
            }

            if (!string.IsNullOrWhiteSpace(editSinhVienVM.Email) && student.NguoiDung != null)
                {
                    student.NguoiDung.Email = editSinhVienVM.Email;
                }

            if (!string.IsNullOrWhiteSpace(editSinhVienVM.SDT) && student.NguoiDung != null)
                {
                    student.NguoiDung.PhoneNumber = editSinhVienVM.SDT;
                }

            if (!string.IsNullOrWhiteSpace(editSinhVienVM.AnhThe) && student.NguoiDung != null)
                {
                    student.NguoiDung.AnhDaiDien = editSinhVienVM.AnhThe;
                }

                // Update AnhThe (ID card / avatar) on student entity
                if (!string.IsNullOrWhiteSpace(editSinhVienVM.AnhThe))
                {
                    student.AnhThe = editSinhVienVM.AnhThe;
                }

                // Update competency profile URL
                if (!string.IsNullOrWhiteSpace(editSinhVienVM.HoSoNangLuc))
                {
                    student.HoSoNangLuc = editSinhVienVM.HoSoNangLuc;
                }

                student.UpdatedAt = DateTime.UtcNow;
                student.UpdatedBy = maSinhVien;

            _context.SinhViens.Update(student);
            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return BaseResult.Fail("Cập nhật thông tin sinh viên thất bại");
            }

            return BaseResult.Success("Cập nhật thông tin sinh viên thành công");
        }
    }
}
