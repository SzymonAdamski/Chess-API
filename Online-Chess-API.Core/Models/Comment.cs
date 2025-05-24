using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Chess_API.Core.Models
{
    [Table("Comments")]
    public class Comment
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(1000)]
    public string Content { get; set; } = string.Empty;
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    // Popraw typ klucza obcego na string!
    // Foreign key to Game
    [Required]
    public string GameId { get; set; } = string.Empty; // Zmiana z int na string
    
    [ForeignKey(nameof(GameId))]
    public virtual ChessGame Game { get; set; } = null!;
    
    public int? UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }
}
}