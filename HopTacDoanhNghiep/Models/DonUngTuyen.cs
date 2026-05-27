using HopTacDoanhNghiep.Enums.HoSo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HopTacDoanhNghiep.Models
{
    public class DonUngTuyen
    {
        [Key]
        public int MaUT { get; set; }
        public string? MaSV { get; set; }
        [ForeignKey(nameof(MaSV))]
        public SinhVien? SinhVien { get; set; }
        public int? MaTTD { get; set; }
        [ForeignKey(nameof(MaTTD))]
        public TinTuyenDung? TinTuyenDung { get; set; }
        public string HoSoUngTuyen { get; set; }
        public HoSoStatus TrangThai { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

    }
}
