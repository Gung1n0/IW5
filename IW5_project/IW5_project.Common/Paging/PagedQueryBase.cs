using IW5_project.IW5_project.Common.Enums;

namespace IW5_project.IW5_project.Common.Paging
{
    public class PagedQueryBase
    { 
        public string? search { get; set; }
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public SortDirection sortDirection { get; set; } = SortDirection.Ascending;
    }
}
