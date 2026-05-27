using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HopTacDoanhNghiep.Models
{
    public class CanBo
    {
        [Key]
        public string MaCB { get; set; }
        public int MaChucVu { get; set; }
        [ForeignKey(nameof(MaChucVu))]
        public ChucVu ChucVu { get; set; }
        public int MaDonVi { get; set; }
        [ForeignKey(nameof(MaDonVi))]
        public DonVi DonVi { get; set; }
        public string MaNguoiDung { get; set; }
        public string? BHTT { get; set; }
        public string? BHTN { get; set; }
        public string? STK { get; set; }
        public string? AnhThe { get; set; }
        public string? GhiChu { get; set; }
        [ForeignKey("MaNguoiDung")]
        public AppUser NguoiDung { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
