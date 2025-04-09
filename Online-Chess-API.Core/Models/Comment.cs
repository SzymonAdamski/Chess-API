using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Chess_API.Core.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Content { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Foreign keys
        public int GameId { get; set; }
        
        [ForeignKey("GameId")]
        public virtual ChessGame Game { get; set; }
        
        public int? UserId { get; set; }
        
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
