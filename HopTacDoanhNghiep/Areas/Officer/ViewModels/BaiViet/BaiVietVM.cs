using HopTacDoanhNghiep.Enums.BaiViet;
using HopTacDoanhNghiep.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HopTacDoanhNghiep.Areas.Officer.ViewModels.BaiViet
{
    public class BaiVietVM
    {
        public string? TieuDe { get; set; }
        public string? AnhMinhHoa { get; set; }
        public string? TacGia { get; set; }
        public string? NoiDung { get; set; }
        public string? Slug { get; set; }
        public string? DanhMucSlug { get; set; }
        public string? DanhMuc { get; set; }
        public BaiVietStatus? TrangThai { get; set; }
        public DateTime? CreatedAt { get; set; }

        public ICollection<BaiVietVM> BaiVietLienQuan { get; set; } = new List<BaiVietVM>();
    }
}
