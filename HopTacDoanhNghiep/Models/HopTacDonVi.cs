using HopTacDoanhNghiep.Enums.HopTac;
using HopTacDoanhNghiep.Enums.HoSo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HopTacDoanhNghiep.Models
{
    public class HopTacDonVi
    {
        [Key]
        public int MaHTDV { get; set; }
        public string MaDN { get; set; }
        [ForeignKey(nameof(MaDN))]
        public DoanhNghiep? DoanhNghiep { get; set; }
        public int? MaDV { get; set; }
        [ForeignKey(nameof(MaDV))]
        public DonVi? DonVi { get; set; }
        public HopTacDonViStatus TrangThai { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
