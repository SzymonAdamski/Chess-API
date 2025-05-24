using Microsoft.EntityFrameworkCore;
using Online_Chess_API.Core.DTOs;
using Online_Chess_API.Core.Interfaces;
using Online_Chess_API.Core.Models;
using Online_Chess_API.Infrastructure.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace Online_Chess_API.Infrastructure.Repositories
{
    public class ChessGameRepository : IChessGameRepository
    {
        private readonly ChessDbContext _context;
        
        public ChessGameRepository(ChessDbContext context)
        {
            _context = context;
        }
        
        public async Task<ChessGame> GetGameByIdAsync(string id)
        {
            return await _context.ChessGames
                .FirstOrDefaultAsync(g => g.GameId == id);
        }

        public async Task<IEnumerable<ChessGame>> GetAllGamesAsync(PaginationDto pagination)
        {
            var query = _context.ChessGames
                .AsQueryable();
            
            // Apply sorting
            if (!string.IsNullOrEmpty(pagination.SortBy))
            {
                var sortPropertyInfo = typeof(ChessGame).GetProperty(pagination.SortBy, 
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                
                if (sortPropertyInfo != null)
                {
                    var parameter = Expression.Parameter(typeof(ChessGame), "x");
                    var propertyExpr = Expression.Property(parameter, sortPropertyInfo);
                    var lambda = Expression.Lambda(propertyExpr, parameter);
                    
                    var methodName = pagination.SortDescending ? "OrderByDescending" : "OrderBy";
                    var resultExpression = Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new[] { typeof(ChessGame), sortPropertyInfo.PropertyType },
                        query.Expression,
                        Expression.Quote(lambda));
                        
                    query = query.Provider.CreateQuery<ChessGame>(resultExpression);
                }
            }
            
            // Apply pagination
            return await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();
        }
        
        public async Task<ChessGame> CreateGameAsync(ChessGame game)
        {
            await _context.ChessGames.AddAsync(game);
            await _context.SaveChangesAsync();
            return game;
        }
        
        public async Task<ChessGame> UpdateGameAsync(ChessGame game)
        {
            _context.ChessGames.Update(game);
            await _context.SaveChangesAsync();
            return game;
        }
        
        public async Task<bool> DeleteGameAsync(string id)
        {
            var game = await _context.ChessGames.FindAsync(id);
            if (game == null)
                return false;
                
            _context.ChessGames.Remove(game);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<int> GetTotalGamesCountAsync()
        {
            return await _context.ChessGames.CountAsync();
        }
        
        public async Task<IEnumerable<ChessGame>> FilterGamesAsync(string property, string value, PaginationDto pagination)
        {
            var query = _context.ChessGames
                .AsQueryable();
            
            // Apply filter
            if (!string.IsNullOrEmpty(property) && !string.IsNullOrEmpty(value))
            {
                var filterPropertyInfo = typeof(ChessGame).GetProperty(property, 
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                
                if (filterPropertyInfo != null)
                {
                    var parameter = Expression.Parameter(typeof(ChessGame), "x");
                    var propertyExpr = Expression.Property(parameter, filterPropertyInfo);
                    var valueExpr = Expression.Constant(value);
                    
                    // For string properties, perform a contains operation
                    if (filterPropertyInfo.PropertyType == typeof(string))
                    {
                        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        var containsExpression = Expression.Call(propertyExpr, containsMethod, valueExpr);
                        var filterLambda = Expression.Lambda<Func<ChessGame, bool>>(containsExpression, parameter);
                        query = query.Where(filterLambda);
                    }
                    else if (filterPropertyInfo.PropertyType == typeof(int))
                    {
                        int intValue;
                        if (int.TryParse(value, out intValue))
                        {
                            var intValueExpr = Expression.Constant(intValue);
                            var equalExpression = Expression.Equal(propertyExpr, intValueExpr);
                            var filterLambda = Expression.Lambda<Func<ChessGame, bool>>(equalExpression, parameter);
                            query = query.Where(filterLambda);
                        }
                        else
                        {
                            // Jeu015Bli nie udau0142o siu0119 przekonwertowau0107 wartou015Bci, zwracamy pusty wynik
                            return Enumerable.Empty<ChessGame>();
                        }
                    }
                    else if (filterPropertyInfo.PropertyType == typeof(bool))
                    {
                        bool boolValue;
                        if (bool.TryParse(value, out boolValue))
                        {
                            var boolValueExpr = Expression.Constant(boolValue);
                            var equalExpression = Expression.Equal(propertyExpr, boolValueExpr);
                            var filterLambda = Expression.Lambda<Func<ChessGame, bool>>(equalExpression, parameter);
                            query = query.Where(filterLambda);
                        }
                    }
                }
            }
            
            // Apply sorting
            if (!string.IsNullOrEmpty(pagination.SortBy))
            {
                var sortPropertyInfo = typeof(ChessGame).GetProperty(pagination.SortBy, 
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                
                if (sortPropertyInfo != null)
                {
                    var sortParameter = Expression.Parameter(typeof(ChessGame), "x");
                    var sortPropertyExpr = Expression.Property(sortParameter, sortPropertyInfo);
                    var sortLambda = Expression.Lambda(sortPropertyExpr, sortParameter);
                    
                    var methodName = pagination.SortDescending ? "OrderByDescending" : "OrderBy";
                    var resultExpression = Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new[] { typeof(ChessGame), sortPropertyInfo.PropertyType },
                        query.Expression,
                        Expression.Quote(sortLambda));
                        
                    query = query.Provider.CreateQuery<ChessGame>(resultExpression);
                }
            }
            
            // Apply pagination
            return await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();
        }
    }
}
