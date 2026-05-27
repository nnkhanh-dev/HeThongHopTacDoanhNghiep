using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HopTacDoanhNghiep.Models
{
    public class ThongBao
    {
        [Key]
        public int MaThongBao { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public string MaNguoiDung { get; set; }
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
