using System.ComponentModel.DataAnnotations;

namespace HopTacDoanhNghiep.Models
{
    public class ChucVu
    {
        [Key]
        public int MaChucVu { get; set; }
        public string TenChucVu { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

        public ICollection<CanBo> CanBos { get; set; }
    }
}
