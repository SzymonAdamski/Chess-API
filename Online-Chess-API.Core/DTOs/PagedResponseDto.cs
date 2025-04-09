namespace Online_Chess_API.Core.DTOs
{
    public class PagedResponseDto<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public Dictionary<string, string> Links { get; set; } = new Dictionary<string, string>();
    }
}
