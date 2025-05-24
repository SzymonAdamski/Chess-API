using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Chess_API.Core.Models
{
    [Table("chess_games")]
    public class ChessGame
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("game_id")]
        [StringLength(50)]
        public string GameId { get; set; } = string.Empty;
        
        [Required]
        [Column("rated")]
        public bool Rated { get; set; }
        
        [Required]
        [Column("turns")]
        public short Turns { get; set; }
        
        [Required]
        [Column("victory_status")]
        [StringLength(11)]
        public string VictoryStatus { get; set; } = string.Empty;
        
        [Required]
        [Column("winner")]
        [StringLength(5)]
        public string Winner { get; set; } = string.Empty;
        
        [Required]
        [Column("time_increment")]
        [StringLength(7)]
        public string TimeIncrement { get; set; } = string.Empty;
        
        [Required]
        [Column("white_id")]
        [StringLength(20)]
        public string WhiteId { get; set; } = string.Empty;
        
        [Required]
        [Column("white_rating")]
        public short WhiteRating { get; set; }
        
        [Required]
        [Column("black_id")]
        [StringLength(20)]
        public string BlackId { get; set; } = string.Empty;
        
        [Required]
        [Column("black_rating")]
        public short BlackRating { get; set; }
        
        [Column("moves")]
        [Required]
        [StringLength(1413)]
        public string Moves { get; set; } = string.Empty;
        
        [Column("opening_code")]
        [Required]
        [StringLength(3)]
        public string OpeningCode { get; set; } = string.Empty;
        
        [Column("opening_moves")]
        [Required]
        public byte OpeningMoves { get; set; }
        
        [Column("opening_fullname")]
        [Required]
        [StringLength(91)]
        public string OpeningFullname { get; set; } = string.Empty;
        
        [Column("opening_shortname")]
        [Required]
        [StringLength(26)]
        public string OpeningShortname { get; set; } = string.Empty;
        
        [Column("opening_response")]
        [StringLength(8)]
        public string? OpeningResponse { get; set; }
        
        [Column("opening_variation")]
        [StringLength(47)]
        public string? OpeningVariation { get; set; }
        
        // Navigation properties
        [NotMapped]
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
