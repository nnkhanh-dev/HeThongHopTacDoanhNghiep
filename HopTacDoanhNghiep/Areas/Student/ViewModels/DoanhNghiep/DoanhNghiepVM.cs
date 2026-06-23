using HopTacDoanhNghiep.Areas.Student.ViewModels.ViecLam;
using HopTacDoanhNghiep.ViewModels.Common;

namespace HopTacDoanhNghiep.Areas.Student.ViewModels.DoanhNghiep
{
    public class DoanhNghiepVM
    {
        public string MaDN { get; set; }
        public string TenHienThi { get; set; }
        public string? Website { get; set; }
        public string? MaSoThue { get; set; }
        public string? TenPhapLy { get; set; }
        public string? Hotline { get; set; }
        public string? EmailCongTy { get; set; }
        public string? Logo { get; set; }
        public string? DiaChi { get; set; }
        public string? GioiThieu { get; set; }
        public int? QuyMoNhanSu { get; set; }

        public PageResult<TinTuyenDungVM>? TinTuyenDung { get; set; } = new PageResult<TinTuyenDungVM>();
    }
}
