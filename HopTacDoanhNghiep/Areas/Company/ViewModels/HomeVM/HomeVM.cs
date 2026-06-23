using HopTacDoanhNghiep.Areas.Company.ViewModels.ViecLam;
using HopTacDoanhNghiep.Areas.Company.ViewModels.BaiViet;

namespace HopTacDoanhNghiep.Areas.Company.ViewModels.HomeVM
{
    public class HomeVM
    {
        public ICollection<BaiVietVM> TinTucs { get; set; } = new List<BaiVietVM>();
        public ICollection<BaiVietVM> ThongBaos { get; set; } = new List<BaiVietVM>();
        public ICollection<BaiVietVM> BaiViets { get; set; } = new List<BaiVietVM>();
    }
}
