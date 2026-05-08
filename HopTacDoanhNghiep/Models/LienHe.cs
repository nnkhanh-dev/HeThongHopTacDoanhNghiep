using HopTacDoanhNghiep.Enums.LienHe;
using System.ComponentModel.DataAnnotations;

namespace HopTacDoanhNghiep.Models
{
    public class LienHe
    {
        [Key]
        public int Id { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string DienThoai { get; set; }
        public string NoiDung { get; set; }
        public LienHeStatus TrangThai { get; set; } = LienHeStatus.ChoXuLy;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
