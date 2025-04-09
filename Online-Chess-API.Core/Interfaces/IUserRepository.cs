using Online_Chess_API.Core.Models;

namespace Online_Chess_API.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> UserExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
    }
}
