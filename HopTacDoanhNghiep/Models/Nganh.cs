using System.ComponentModel.DataAnnotations;

namespace HopTacDoanhNghiep.Models
{
    public class Nganh
    {
        [Key]
        public int Id { get; set; }
        public string MaNganh { get; set; } 
        public string TenNganh { get; set; }
        public string? TenChuyenNganh { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        public ICollection<LinhVucNganh> LinhVucNganhs { get; set; }
    }
}
