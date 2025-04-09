namespace Online_Chess_API.Core.DTOs
{
    public class PaginationDto
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;
        
        public int PageNumber { get; set; } = 1;
        
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        
        public string SortBy { get; set; } = "GameId";
        
        public bool SortDescending { get; set; } = false;
    }
}
