namespace IW5_project.Api.Common.Models.Common  
{
    public record PagedResult<T>
    {
        public required IList<T> Items { get; init; }
        public required int Page { get; init; }
        public required int PageSize { get; init; }
        public required int TotalCount { get; init; }

        public int TotalPages => PageSize > 0
            ? (int)Math.Ceiling(TotalCount / (double)PageSize)
            : 0;

        public PagedResult<TOut> Map<TOut>(Func<T, TOut> selector) => new()
        {
            Items = Items.Select(selector).ToList(),
            Page = Page,
            PageSize = PageSize,
            TotalCount = TotalCount
        };
    }
}
