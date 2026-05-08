using HopTacDoanhNghiep.Enums.ViecLam;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HopTacDoanhNghiep.Models
{
    public class ViecLam
    {
        [Key]
        public int Id { get; set; }
        public string TieuDe { get; set; }
        public string Slug { get; set; }
        public string MoTa { get; set; }
        public string YeuCau { get; set; }
        public string UuTien { get; set; }
        public string QuyenLoi { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal LuongToiThieu { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal LuongToiDa { get; set; }
        public string DiaDiem { get; set; }
        public string TuKhoa { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayHetHan { get; set; }
        public ViecLamType LoaiViecLam { get; set; }
        public DoiTuongUngTuyen DoiTuongUngTuyen { get; set; }
        public TrinhDoType TrinhDo { get; set; }
        public ViecLamStatus Status { get; set; }
        public Guid? DoanhNghiepId { get; set; }
        [ForeignKey(nameof(DoanhNghiepId))]
        public DoanhNghiep? DoanhNghiep { get; set; }
        public int LinhVucId { get; set; }
        [ForeignKey(nameof(LinhVucId))]
        public LinhVuc LinhVuc { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

        public ICollection<SinhVienViecLam> SinhVienViecLams { get; set; }
        public ICollection<LuuTru> LuuTrus { get; set; }
        public ICollection<LichPhongVan> LichPhongVans  { get; set; }
    }
}
