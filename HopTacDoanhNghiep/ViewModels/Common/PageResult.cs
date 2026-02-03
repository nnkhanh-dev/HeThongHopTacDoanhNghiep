namespace HopTacDoanhNghiep.ViewModels.Common
{
    public class PageResult<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public IEnumerable<T> Records { get; set; }
    }
}
