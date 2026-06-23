using HopTacDoanhNghiep.Areas.Officer.ViewModels.BaiViet;

namespace HopTacDoanhNghiep.Areas.Officer.ViewModels.HomeVM
{
    public class HomeVM
    {
        public ICollection<BaiVietVM> TinTucs { get; set; } = new List<BaiVietVM>();
        public ICollection<BaiVietVM> ThongBaos { get; set; } = new List<BaiVietVM>();
        public ICollection<BaiVietVM> BaiViets { get; set; } = new List<BaiVietVM>();
    }
}
