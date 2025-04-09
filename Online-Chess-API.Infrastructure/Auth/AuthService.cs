using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Online_Chess_API.Core.DTOs;
using Online_Chess_API.Core.Interfaces;
using Online_Chess_API.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Online_Chess_API.Infrastructure.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        
        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        
        public async Task<User> RegisterAsync(UserRegisterDto userDto)
        {
            // Check if username or email already exists
            if (await _userRepository.UserExistsAsync(userDto.Username))
            {
                throw new InvalidOperationException("Username already exists");
            }
            
            if (await _userRepository.EmailExistsAsync(userDto.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }
            
            // Create new user
            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = CreatePasswordHash(userDto.Password),
                CreatedAt = DateTime.UtcNow
            };
            
            return await _userRepository.CreateUserAsync(user);
        }
        
        public async Task<string> LoginAsync(UserLoginDto userDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(userDto.Username);
            
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }
            
            if (!VerifyPasswordHash(userDto.Password, user.PasswordHash))
            {
                throw new InvalidOperationException("Invalid password");
            }
            
            // Update last login
            user.LastLogin = DateTime.UtcNow;
            await _userRepository.UpdateUserAsync(user);
            
            // Generate and return token
            return GenerateJwtToken(user);
        }
        
        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
        public bool VerifyPasswordHash(string password, string storedHash)
        {
            using var hmac = new HMACSHA512();
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            
            var hash = new StringBuilder();
            foreach (var b in computedHash)
            {
                hash.Append(b.ToString("x2"));
            }
            
            return hash.ToString() == storedHash;
        }
        
        public string CreatePasswordHash(string password)
        {
            using var hmac = new HMACSHA512();
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            
            var hash = new StringBuilder();
            foreach (var b in passwordHash)
            {
                hash.Append(b.ToString("x2"));
            }
            
            return hash.ToString();
        }
    }
}
