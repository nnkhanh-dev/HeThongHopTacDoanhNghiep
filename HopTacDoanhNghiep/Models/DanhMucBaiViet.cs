using System.ComponentModel.DataAnnotations;

namespace HopTacDoanhNghiep.Models
{
    public class DanhMucBaiViet
    {
        [Key]
        public int Id { get; set; }
        public string Ten { get; set; }
        public string? MoTa { get; set; }
        public string Slug { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public ICollection<BaiViet> BaiViets { get; set; } = new List<BaiViet>();
    }
}
