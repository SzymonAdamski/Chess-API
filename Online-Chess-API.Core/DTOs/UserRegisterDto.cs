using System.ComponentModel.DataAnnotations;

namespace Online_Chess_API.Core.DTOs
{
    public class UserRegisterDto
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
