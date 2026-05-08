namespace HopTacDoanhNghiep.Models
{
    public class DangKyPhongVan
    {
        public int LichPhongVanId { get; set; }
        public LichPhongVan LichPhongVan { get; set; }
        public int SinhVienViecLamId { get; set; }
        public SinhVienViecLam SinhVienViecLam { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
