using HopTacDoanhNghiep.Models;

namespace HopTacDoanhNghiep.Areas.Company.ViewModels.LinhVuc
{
    public class LinhVucVM
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public string? MoTa { get; set; }
        public string Slug { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
