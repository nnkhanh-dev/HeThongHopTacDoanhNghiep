using HopTacDoanhNghiep.Areas.Student.ViewModels.SinhVien;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Student.Services
{
    public interface ISinhVienStudent
    {
        Task<BaseResult<SinhVienVM>> GetStudentInfo(string maSinhVien);
        Task<BaseResult> EditStudentInfo(string maSinhVien, EditSinhVienVM editSinhVienVM);
    }
}
