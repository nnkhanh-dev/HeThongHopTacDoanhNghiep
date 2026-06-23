namespace HopTacDoanhNghiep.Areas.Admin.ViewModels.Dashboard
{
    public class DashboardDataVM
    {
        public int CanBos { get; set; }
        public int DoanhNghieps { get; set; }
        public int SinhViens { get; set; }
        public int TinTuyenDungs { get; set; }
        public List<DashboardDataByLabelVM> TinTuyenDungByMonth { get; set; } = new List<DashboardDataByLabelVM>();
        public List<DashboardDataByLabelVM> DoanhNghiepByMonth { get; set; } = new List<DashboardDataByLabelVM>();
        public List<DashboardDataByLabelVM> TintuyenDungByDoanhNghiep { get; set; } = new List<DashboardDataByLabelVM>();
        public List<DashboardDataByLabelVM> DonUngTuyenByMonth { get; set; } = new List<DashboardDataByLabelVM>();
    }

    public class DashboardDataByLabelVM
    {
        public string Label { get; set; }
        public int Count { get; set; }
    }
}
