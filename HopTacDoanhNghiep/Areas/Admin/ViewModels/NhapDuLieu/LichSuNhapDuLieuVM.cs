using HopTacDoanhNghiep.Enums.NhapDuLieu;

namespace HopTacDoanhNghiep.Areas.Admin.ViewModels.NhapDuLieu
{
    public class LichSuNhapDuLieuVM
    {
        public int? Id { get; set; }
        public int? TongDuLieu { get; set; }
        public int? ThanhCong { get; set; }
        public int? ThatBai { get; set; }
        public NhapDuLieuStatus? TrangThai { get; set; } 
        public NhapDuLieuType? PhanLoai { get; set; }
        public string? DuongDanFileGoc { get; set; }
        public string? DuongDanFileLoi { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }
}
