using HopTacDoanhNghiep.Areas.Student.ViewModels.ViecLam;
using HopTacDoanhNghiep.Enums.HoSo;
using HopTacDoanhNghiep.Enums.ViecLam;
using System.ComponentModel.DataAnnotations.Schema;

namespace HopTacDoanhNghiep.Areas.Student.ViewModels.DonUngTuyen
{
    public class DonUngTuyenVM
    {
        public int MaUT { get; set; }
        public string? MaSV { get; set; }
        public string? TenSinhVien { get; set; }
        public int? MaTTD { get; set; }
        public TinTuyenDungVM TinTuyenDung { get; set; } = new TinTuyenDungVM();
        public string? HoSoUngTuyen { get; set; }
        public HoSoStatus TrangThai { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
