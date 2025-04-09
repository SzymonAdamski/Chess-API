using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Online_Chess_API.Core.Models;
using System.Security.Cryptography;
using System.Text;

namespace Online_Chess_API.Infrastructure.Data
{
    public class DbSeeder
    {
        public static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ChessDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbSeeder>>();

            try
            {
                await dbContext.Database.MigrateAsync();
                
                // Seed users if none exist
                if (!await dbContext.Users.AnyAsync())
                {
                    logger.LogInformation("Seeding users...");
                    await SeedUsersAsync(dbContext);
                }
                
                // Seed chess games if none exist
                if (!await dbContext.ChessGames.AnyAsync())
                {
                    logger.LogInformation("Seeding chess games...");
                    await SeedChessGamesAsync(dbContext);
                }
                
                // Seed comments if none exist
                if (!await dbContext.Comments.AnyAsync())
                {
                    logger.LogInformation("Seeding comments...");
                    await SeedCommentsAsync(dbContext);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
        
        private static async Task SeedUsersAsync(ChessDbContext dbContext)
        {
            var users = new List<User>
            {
                new User
                {
                    Username = "testuser1",
                    Email = "user1@example.com",
                    PasswordHash = CreatePasswordHash("Password123"),
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    Username = "testuser2",
                    Email = "user2@example.com",
                    PasswordHash = CreatePasswordHash("Password123"),
                    CreatedAt = DateTime.UtcNow
                }
            };
            
            await dbContext.Users.AddRangeAsync(users);
            await dbContext.SaveChangesAsync();
        }
        
        private static async Task SeedChessGamesAsync(ChessDbContext dbContext)
        {
            var games = new List<ChessGame>
            {
                new ChessGame
                {
                    Rated = true,
                    Turns = 20,
                    VictoryStatus = "Checkmate",
                    Winner = "white",
                    TimeIncrement = "5+0",
                    WhiteId = 1001,
                    WhiteRating = 1800,
                    BlackId = 1002,
                    BlackRating = 1750,
                    Moves = "e4 e5 Nf3 Nc6 Bc4 Nf6 d3 Bc5 O-O d6 h3 O-O",
                    OpeningCode = "C50",
                    OpeningMoves = "e4 e5 Nf3 Nc6 Bc4",
                    OpeningFullname = "Italian Game",
                    OpeningShortname = "Italian",
                    OpeningResponse = "Two Knights Defense",
                    OpeningVariation = "Classical Variation"
                },
                new ChessGame
                {
                    Rated = true,
                    Turns = 30,
                    VictoryStatus = "Resignation",
                    Winner = "black",
                    TimeIncrement = "10+5",
                    WhiteId = 1003,
                    WhiteRating = 2000,
                    BlackId = 1004,
                    BlackRating = 2100,
                    Moves = "d4 Nf6 c4 e6 Nc3 Bb4 e3 O-O Bd3 d5 cxd5 exd5 Nge2 c5 O-O cxd4 exd4 Nc6",
                    OpeningCode = "E48",
                    OpeningMoves = "d4 Nf6 c4 e6 Nc3 Bb4",
                    OpeningFullname = "Nimzo-Indian Defense",
                    OpeningShortname = "Nimzo-Indian",
                    OpeningResponse = "Normal Variation",
                    OpeningVariation = "4.e3 O-O 5.Bd3 d5"
                }
            };
            
            await dbContext.ChessGames.AddRangeAsync(games);
            await dbContext.SaveChangesAsync();
        }
        
        private static async Task SeedCommentsAsync(ChessDbContext dbContext)
        {
            var users = await dbContext.Users.ToListAsync();
            var games = await dbContext.ChessGames.ToListAsync();
            
            if (users.Count == 0 || games.Count == 0)
                return;
                
            var comments = new List<Comment>
            {
                new Comment
                {
                    Content = "This was a fantastic game with a brilliant checkmate at the end!",
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    GameId = games[0].GameId,
                    UserId = users[0].Id
                },
                new Comment
                {
                    Content = "The opening was played very well by both sides.",
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    GameId = games[0].GameId,
                    UserId = users[1].Id
                },
                new Comment
                {
                    Content = "That knight sacrifice on move 15 was unexpected!",
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    GameId = games[1].GameId,
                    UserId = users[0].Id
                }
            };
            
            await dbContext.Comments.AddRangeAsync(comments);
            await dbContext.SaveChangesAsync();
        }
        
        private static string CreatePasswordHash(string password)
        {
            using var hmac = new HMACSHA512();
            var passwordSalt = hmac.Key;
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
