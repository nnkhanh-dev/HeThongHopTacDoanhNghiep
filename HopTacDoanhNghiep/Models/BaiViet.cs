using HopTacDoanhNghiep.Enums.BaiViet;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HopTacDoanhNghiep.Models
{
    public class BaiViet
    {
        [Key]
        public int MaBaiViet { get; set; }
        public string TieuDe { get; set; }
        public string? AnhMinhHoa { get; set; }
        public string TacGia { get; set; }
        public string NoiDung { get; set; }
        public string Slug { get; set; }
        public string? TuKhoa { get; set; }
        public BaiVietStatus TrangThai { get; set; }
        public int MaDanhMuc { get; set; }
        [ForeignKey(nameof(MaDanhMuc))]
        public DanhMucBaiViet DanhMuc { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public AppUser NguoiDung { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
