using Online_Chess_API.Core.DTOs;
using Online_Chess_API.Core.Models;

namespace Online_Chess_API.Core.Interfaces
{
    public interface IChessGameRepository
    {
        Task<IEnumerable<ChessGame>> GetAllGamesAsync(PaginationDto pagination);
        Task<ChessGame> GetGameByIdAsync(int id);
        Task<ChessGame> CreateGameAsync(ChessGame game);
        Task<ChessGame> UpdateGameAsync(ChessGame game);
        Task<bool> DeleteGameAsync(int id);
        Task<int> GetTotalGamesCountAsync();
        Task<IEnumerable<ChessGame>> FilterGamesAsync(string property, string value, PaginationDto pagination);
    }
}
