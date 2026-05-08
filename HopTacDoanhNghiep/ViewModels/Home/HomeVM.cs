using HopTacDoanhNghiep.ViewModels.BaiViet;
using HopTacDoanhNghiep.ViewModels.ViecLam;

namespace HopTacDoanhNghiep.ViewModels.Home
{
    public class HomeVM
    {
        public ICollection<BaiVietVM> TinTucs { get; set; } = new List<BaiVietVM>();
        public ICollection<BaiVietVM> ThongBaos { get; set; } = new List<BaiVietVM>();
        public ICollection<ViecLamVM> ViecLams { get; set; } = new List<ViecLamVM>();
        public ICollection<BaiVietVM> BaiViets { get; set; } = new List<BaiVietVM>();
    }
}
