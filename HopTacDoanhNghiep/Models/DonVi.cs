using HopTacDoanhNghiep.Enums.DonVi;
using System.ComponentModel.DataAnnotations;

namespace HopTacDoanhNghiep.Models
{
    public class DonVi
    {
        [Key]
        public int MaDV { get; set; }
        public string TenDV { get; set; }
        public string? Tel { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public DonViStatus TrangThai { get; set; }
        public bool NhanDoiTac { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

        public ICollection<CanBo> CanBos { get; set; }
    }
}
