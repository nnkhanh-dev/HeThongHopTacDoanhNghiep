using HopTacDoanhNghiep.Enums.ViecLam;
using System.ComponentModel.DataAnnotations;

namespace HopTacDoanhNghiep.Areas.Company.ViewModels.ViecLam
{
    public class TinTuyenDungEditVM
    {
        [Required(ErrorMessage = "Tiêu đề không thể để trống")]
        public string TieuDe { get; set; }
        [Required(ErrorMessage = "Mô tả không thể để trống")]
        public string MoTa { get; set; }
        [Required(ErrorMessage = "Yêu cầu không thể để trống")]
        public string YeuCau { get; set; }
        public string? UuTien { get; set; }
        [Required(ErrorMessage = "Quyền lợi không thể để trống")]
        public string QuyenLoi { get; set; }
        [Required(ErrorMessage = "Lương tối thiểu không thể để trống")]
        [Range(0, double.MaxValue)]
        public decimal? LuongToiThieu { get; set; }
        [Required(ErrorMessage = "Lương tối đa không thể để trống")]
        [Range(0, double.MaxValue)]
        public decimal? LuongToiDa { get; set; }
        [Required(ErrorMessage = "Địa điểm không thể để trống")]
        public string DiaDiem { get; set; }
        public string? TuKhoa { get; set; }
        [Required(ErrorMessage = "Ngày bắt đầu không thể để trống")]
        public DateTime? NgayBatDau { get; set; }
        [Required(ErrorMessage = "Ngày hết hạn không thể để trống")]
        public DateTime? NgayHetHan { get; set; }
        [Required(ErrorMessage = "Trình độ không thể để trống")]
        public ViecLamType? LoaiViecLam { get; set; }
        [Required(ErrorMessage = "Đối tượng ứng tuyển không thể để trống")]
        public DoiTuongUngTuyen? DoiTuongUngTuyen { get; set; }
        [Required(ErrorMessage = "Trình độ không thể để trống")]
        public TrinhDoType? TrinhDo { get; set; }
        [Required(ErrorMessage = "Trạng thái không thể để trống")]
        public ViecLamStatus? Status { get; set; }
        public string? MaDoanhNghiep { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
