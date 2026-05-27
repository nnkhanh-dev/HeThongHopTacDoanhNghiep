using Microsoft.AspNetCore.Http;

namespace HopTacDoanhNghiep.Areas.Student.ViewModels.DonUngTuyen
{
    public class DonUngTuyenCreateVM
    {
        public int MaTTD { get; set; }
        public string? MaSV { get; set; }
        public string? HoSoUngTuyen { get; set; }
    }
}
