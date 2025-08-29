namespace BaseDotnet.Core.Models
{
    public class PagedResult<T>
    {
        public List<T> Data { get; }
        public int TotalCount { get; }
        public int? Page { get; }
        public int? PageSize { get; }
        public int? TotalPages { get; }

        // Konstruktor untuk semua properti
        public PagedResult(List<T> data, int totalCount, int? page = null, int? pageSize = null)
        {
            Data = data;
            TotalCount = totalCount;

            if (page.HasValue && pageSize.HasValue)
            {
                Page = page;
                PageSize = pageSize;
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize.Value);
            }
        }
    }
}
