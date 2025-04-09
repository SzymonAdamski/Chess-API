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
        
        public async Task<IEnumerable<ChessGame>> GetAllGamesAsync(PaginationDto pagination)
        {
            var query = _context.ChessGames.AsQueryable();
            
            // Apply sorting
            if (!string.IsNullOrEmpty(pagination.SortBy))
            {
                var propertyInfo = typeof(ChessGame).GetProperty(pagination.SortBy, 
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                
                if (propertyInfo != null)
                {
                    var parameter = Expression.Parameter(typeof(ChessGame), "x");
                    var property = Expression.Property(parameter, propertyInfo);
                    var lambda = Expression.Lambda(property, parameter);
                    
                    var methodName = pagination.SortDescending ? "OrderByDescending" : "OrderBy";
                    var resultExpression = Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new[] { typeof(ChessGame), propertyInfo.PropertyType },
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
        
        public async Task<ChessGame> GetGameByIdAsync(int id)
        {
            return await _context.ChessGames
                .Include(g => g.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(g => g.GameId == id);
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
        
        public async Task<bool> DeleteGameAsync(int id)
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
            var query = _context.ChessGames.AsQueryable();
            
            // Apply filter
            if (!string.IsNullOrEmpty(property) && !string.IsNullOrEmpty(value))
            {
                var propertyInfo = typeof(ChessGame).GetProperty(property, 
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                
                if (propertyInfo != null)
                {
                    var parameter = Expression.Parameter(typeof(ChessGame), "x");
                    var propertyExpr = Expression.Property(parameter, propertyInfo);
                    var valueExpr = Expression.Constant(value);
                    
                    // For string properties, perform a contains operation
                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        var containsExpression = Expression.Call(propertyExpr, containsMethod, valueExpr);
                        var lambda = Expression.Lambda<Func<ChessGame, bool>>(containsExpression, parameter);
                        query = query.Where(lambda);
                    }
                    else if (propertyInfo.PropertyType == typeof(int))
                    {
                        int intValue;
                        if (int.TryParse(value, out intValue))
                        {
                            var intValueExpr = Expression.Constant(intValue);
                            var equalExpression = Expression.Equal(propertyExpr, intValueExpr);
                            var lambda = Expression.Lambda<Func<ChessGame, bool>>(equalExpression, parameter);
                            query = query.Where(lambda);
                        }
                    }
                    else if (propertyInfo.PropertyType == typeof(bool))
                    {
                        bool boolValue;
                        if (bool.TryParse(value, out boolValue))
                        {
                            var boolValueExpr = Expression.Constant(boolValue);
                            var equalExpression = Expression.Equal(propertyExpr, boolValueExpr);
                            var lambda = Expression.Lambda<Func<ChessGame, bool>>(equalExpression, parameter);
                            query = query.Where(lambda);
                        }
                    }
                }
            }
            
            // Apply sorting
            if (!string.IsNullOrEmpty(pagination.SortBy))
            {
                var propertyInfo = typeof(ChessGame).GetProperty(pagination.SortBy, 
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                
                if (propertyInfo != null)
                {
                    var parameter = Expression.Parameter(typeof(ChessGame), "x");
                    var propertySort = Expression.Property(parameter, propertyInfo);
                    var lambda = Expression.Lambda(propertySort, parameter);
                    
                    var methodName = pagination.SortDescending ? "OrderByDescending" : "OrderBy";
                    var resultExpression = Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new[] { typeof(ChessGame), propertyInfo.PropertyType },
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
    }
}