using Microsoft.EntityFrameworkCore;
using Online_Chess_API.Core.DTOs;
using Online_Chess_API.Core.Interfaces;
using Online_Chess_API.Core.Models;
using Online_Chess_API.Infrastructure.Data;

namespace Online_Chess_API.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ChessDbContext _context;
        
        public CommentRepository(ChessDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Comment>> GetCommentsByGameIdAsync(int gameId, PaginationDto pagination)
        {
            var query = _context.Comments
                .Include(c => c.User)
                .Where(c => c.GameId == gameId)
                .AsQueryable();
                
            // Apply sorting
            query = pagination.SortDescending
                ? query.OrderByDescending(c => c.CreatedAt)
                : query.OrderBy(c => c.CreatedAt);
                
            // Apply pagination
            return await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();
        }
        
        public async Task<Comment> GetCommentByIdAsync(int id)
        {
            return await _context.Comments
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        
        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }
        
        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {
            comment.UpdatedAt = DateTime.UtcNow;
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return comment;
        }
        
        public async Task<bool> DeleteCommentAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return false;
                
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<int> GetTotalCommentsCountForGameAsync(int gameId)
        {
            return await _context.Comments.CountAsync(c => c.GameId == gameId);
        }
    }
}
