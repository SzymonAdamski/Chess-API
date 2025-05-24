namespace Online_Chess_API.Core.DTOs
{
    public class ChessGameDto
    {
        public int GameId { get; set; }
        public bool Rated { get; set; }
        public int Turns { get; set; }
        public string VictoryStatus { get; set; } = string.Empty;
        public string Winner { get; set; } = string.Empty;
        public string TimeIncrement { get; set; } = string.Empty;
        public string WhiteId { get; set; } = string.Empty;
        public int WhiteRating { get; set; }
        public string BlackId { get; set; } = string.Empty;
        public int BlackRating { get; set; }
        public string Moves { get; set; } = string.Empty;
        public string OpeningCode { get; set; } = string.Empty;
        public int OpeningMoves { get; set; }
        public string OpeningFullname { get; set; } = string.Empty;
        public string OpeningShortname { get; set; } = string.Empty;
        public string? OpeningResponse { get; set; }
        public string? OpeningVariation { get; set; }
    }
}
