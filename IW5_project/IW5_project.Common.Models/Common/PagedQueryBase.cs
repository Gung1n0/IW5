using IW5_project.Common.Enums;

namespace IW5_project.Api.Common.Models.Common
{
    public class PagedQueryBase
    { 
        public string? search { get; set; }
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public SortDirection sortDirection { get; set; } = SortDirection.Ascending;
        
        public PagedQueryBase(
        string? search = null,
        int page = 1,
        int pageSize = 10,
        SortDirection sortDirection = SortDirection.Ascending)
        {
            this.search = search;
            this.page = page;
            this.pageSize = pageSize;
            this.sortDirection = sortDirection;
        }
    }
}
