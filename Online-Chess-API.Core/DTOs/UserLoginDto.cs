using System.ComponentModel.DataAnnotations;

namespace Online_Chess_API.Core.DTOs
{
    public class UserLoginDto
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
