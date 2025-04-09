using Online_Chess_API.Core.DTOs;
using Online_Chess_API.Core.Models;

namespace Online_Chess_API.Core.Interfaces
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(UserRegisterDto userDto);
        Task<string> LoginAsync(UserLoginDto userDto);
        string GenerateJwtToken(User user);
        bool VerifyPasswordHash(string password, string passwordHash);
        string CreatePasswordHash(string password);
    }
}
