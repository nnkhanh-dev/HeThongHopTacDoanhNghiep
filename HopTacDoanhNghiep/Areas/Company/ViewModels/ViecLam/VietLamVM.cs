using HopTacDoanhNghiep.Enums.ViecLam;
using HopTacDoanhNghiep.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HopTacDoanhNghiep.Areas.Company.ViewModels.ViecLam
{
    public class VietLamVM
    {
        public int? Id { get; set; }
        public string? TieuDe { get; set; }
        public string? Slug { get; set; }
        public string? MoTa { get; set; }
        public string? YeuCau { get; set; }
        public string? UuTien { get; set; }
        public string? QuyenLoi { get; set; }
        public decimal? LuongToiThieu { get; set; }
        public decimal? LuongToiDa { get; set; }
        public string? DiaDiem { get; set; }
        public string? TuKhoa { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayHetHan { get; set; }
        public ViecLamType? LoaiViecLam { get; set; }
        public DoiTuongUngTuyen? DoiTuongUngTuyen { get; set; }
        public TrinhDoType? TrinhDo { get; set; }
        public ViecLamStatus? Status { get; set; }
        public Guid? DoanhNghiepId { get; set; }
        public string? DoanhNghiep { get; set; }
        public int? LinhVucId { get; set; }
        public string? LinhVuc { get; set; }
        public string? LinhVucSlug { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
}
