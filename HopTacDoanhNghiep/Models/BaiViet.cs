using HopTacDoanhNghiep.Enums.BaiViet;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HopTacDoanhNghiep.Models
{
    public class BaiViet
    {
        [Key]
        public int Id { get; set; }
        public string TieuDe { get; set; }
        public string? AnhMinhHoa { get; set; }
        public string TacGia { get; set; }
        public string NoiDung { get; set; }
        public string Slug { get; set; }
        public BaiVietStatus TrangThai { get; set; }
        public int DanhMucId { get; set; }
        [ForeignKey(nameof(DanhMucId))]
        public DanhMucBaiViet DanhMuc { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
