using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HopTacDoanhNghiep.Models
{
    public class LichPhongVan
    {
        [Key]
        public int Id { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }
        public string DiaDiem { get; set; }
        public int SoLuongUngVien { get; set; }
        public int ViecLamId { get; set; }
        [ForeignKey("ViecLamId")]
        public ViecLam ViecLam { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

        public ICollection<DangKyPhongVan> DangKyPhongVans { get; set; }
    }
}
