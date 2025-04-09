using System.ComponentModel.DataAnnotations;

namespace Online_Chess_API.Core.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string PasswordHash { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastLogin { get; set; }
        
        // Navigation properties
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
