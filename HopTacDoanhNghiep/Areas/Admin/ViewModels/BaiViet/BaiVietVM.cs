using HopTacDoanhNghiep.Enums.BaiViet;

namespace HopTacDoanhNghiep.Areas.Admin.ViewModels.BaiViet
{
    public class BaiVietVM
    {
        public int? Id { get; set; }
        public string? TieuDe { get; set; }
        public string? AnhMinhHoa { get; set; }
        public string? TacGia { get; set; }
        public string? NoiDung { get; set; }
        public string? Slug { get; set; }
        public BaiVietStatus? TrangThai { get; set; }
        public string? TuKhoa { get; set; }
        public int? DanhMucId { get; set; }
        public string? DanhMuc { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
