namespace IW5_project.API.Common
{
    public class PagedQueryBase
    { 
        public string? search { get; set; }
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public SortDirection sortDirection { get; set; } = SortDirection.Ascending;
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }
}
