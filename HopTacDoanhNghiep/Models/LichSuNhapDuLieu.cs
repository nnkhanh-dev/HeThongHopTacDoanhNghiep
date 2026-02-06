using HopTacDoanhNghiep.Enums.NhapDuLieu;
using System.ComponentModel.DataAnnotations;

namespace HopTacDoanhNghiep.Models
{
    public class LichSuNhapDuLieu
    {
        [Key]
        public int Id { get; set; }
        public int? TongDuLieu{ get; set; }
        public int? ThanhCong { get; set; }
        public int? ThatBai { get; set; }
        public NhapDuLieuStatus TrangThai { get; set; } = NhapDuLieuStatus.ChoXuLy;
        public NhapDuLieuType PhanLoai { get; set; }
        public string DuongDanFileGoc { get; set; }
        public string? DuongDanFileLoi { get; set; }
        public string? GhiChu { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }
}
