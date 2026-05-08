namespace HopTacDoanhNghiep.Models
{
    public class LinhVucNganh
    {
        public int LinhVucId { get; set; }
        public LinhVuc LinhVuc { get; set; }

        public int NganhId { get; set; }
        public Nganh Nganh { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }  
    }
}
