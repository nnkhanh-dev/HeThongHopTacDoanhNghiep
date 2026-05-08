using HopTacDoanhNghiep.Enums.HoSo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HopTacDoanhNghiep.Models
{
    public class SinhVienViecLam
    {
        [Key]
        public int Id { get; set; }
        public Guid? SinhVienId { get; set; }
        [ForeignKey(nameof(SinhVienId))]
        public SinhVien? SinhVien { get; set; }
        public int? ViecLamId { get; set; }
        [ForeignKey(nameof(ViecLamId))]
        public ViecLam? ViecLam { get; set; }
        public string HoSoUngTuyen { get; set; }
        public HoSoStatus TrangThai { get; set; }
        public HoSoType LoaiHoSo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

        public ICollection<DangKyPhongVan> DangKyPhongVans { get; set; }
    }
}
