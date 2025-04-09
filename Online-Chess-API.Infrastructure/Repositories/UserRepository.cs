using Microsoft.EntityFrameworkCore;
using Online_Chess_API.Core.Interfaces;
using Online_Chess_API.Core.Models;
using Online_Chess_API.Infrastructure.Data;

namespace Online_Chess_API.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ChessDbContext _context;
        
        public UserRepository(ChessDbContext context)
        {
            _context = context;
        }
        
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        
        public async Task<User> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        
        public async Task<User> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
        
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;
                
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }
        
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
