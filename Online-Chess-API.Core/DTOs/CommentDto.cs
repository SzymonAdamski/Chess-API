using System.ComponentModel.DataAnnotations;

namespace Online_Chess_API.Core.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        
        [Required]
        public string Content { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        public int GameId { get; set; }
        
        public int? UserId { get; set; }
        
        public string Username { get; set; }
    }
}
