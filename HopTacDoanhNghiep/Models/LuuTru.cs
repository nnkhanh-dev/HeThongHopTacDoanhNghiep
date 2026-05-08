namespace HopTacDoanhNghiep.Models
{
    public class LuuTru
    {
        public Guid SinhVienId { get; set; }
        public SinhVien SinhVien { get; set; } 

        public int ViecLamId { get; set; }
        public ViecLam ViecLam { get; set; } 

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
