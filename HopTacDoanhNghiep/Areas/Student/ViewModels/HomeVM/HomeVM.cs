using HopTacDoanhNghiep.Areas.Student.ViewModels.ViecLam;
using HopTacDoanhNghiep.Areas.Student.ViewModels.BaiViet;

namespace HopTacDoanhNghiep.Areas.Student.ViewModels.HomeVM
{
    public class HomeVM
    {
        public ICollection<BaiVietVM> TinTucs { get; set; } = new List<BaiVietVM>();
        public ICollection<BaiVietVM> ThongBaos { get; set; } = new List<BaiVietVM>();
        public ICollection<TinTuyenDungVM> ViecLams { get; set; } = new List<TinTuyenDungVM>();
        public ICollection<BaiVietVM> BaiViets { get; set; } = new List<BaiVietVM>();
    }
}
