using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Chess_API.Core.Models
{
    public class ChessGame
    {
        [Key]
        public int GameId { get; set; }
        
        public bool Rated { get; set; }
        
        public int Turns { get; set; }
        
        public string VictoryStatus { get; set; }
        
        public string Winner { get; set; }
        
        public string TimeIncrement { get; set; }
        
        public int WhiteId { get; set; }
        
        public int WhiteRating { get; set; }
        
        public int BlackId { get; set; }
        
        public int BlackRating { get; set; }
        
        public string Moves { get; set; }
        
        public string OpeningCode { get; set; }
        
        public string OpeningMoves { get; set; }
        
        public string OpeningFullname { get; set; }
        
        public string OpeningShortname { get; set; }
        
        public string OpeningResponse { get; set; }
        
        public string OpeningVariation { get; set; }
        
        // Navigation properties
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
