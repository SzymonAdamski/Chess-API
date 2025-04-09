using Online_Chess_API.Core.DTOs;
using Online_Chess_API.Core.Models;

namespace Online_Chess_API.Core.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetCommentsByGameIdAsync(int gameId, PaginationDto pagination);
        Task<Comment> GetCommentByIdAsync(int id);
        Task<Comment> CreateCommentAsync(Comment comment);
        Task<Comment> UpdateCommentAsync(Comment comment);
        Task<bool> DeleteCommentAsync(int id);
        Task<int> GetTotalCommentsCountForGameAsync(int gameId);
    }
}
