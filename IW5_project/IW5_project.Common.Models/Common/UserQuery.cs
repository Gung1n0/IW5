using IW5_project.Common.Models.Enums;

namespace IW5_project.Common.Models.Common;

public class UserQuery : PagedQueryBase
{
    public UserQuery(
        string? search = null,
        int page = 1,
        int pageSize = 10,
        SortDirection sortDirection = SortDirection.Ascending)
        : base(search, page, pageSize, sortDirection)
    {
    }
}