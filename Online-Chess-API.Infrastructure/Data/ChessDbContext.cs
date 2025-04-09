using Microsoft.EntityFrameworkCore;
using Online_Chess_API.Core.Models;

namespace Online_Chess_API.Infrastructure.Data
{
    public class ChessDbContext : DbContext
    {
        public ChessDbContext(DbContextOptions<ChessDbContext> options) 
            : base(options)
        {
        }
        
        public DbSet<ChessGame> ChessGames { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure relationships
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Game)
                .WithMany(g => g.Comments)
                .HasForeignKey(c => c.GameId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.SetNull);
                
            // Configure chess game properties
            modelBuilder.Entity<ChessGame>()
                .Property(g => g.GameId)
                .ValueGeneratedOnAdd();
                
            modelBuilder.Entity<ChessGame>()
                .ToTable("ChessGames");
                
            modelBuilder.Entity<User>()
                .ToTable("Users");
                
            modelBuilder.Entity<Comment>()
                .ToTable("Comments");
        }
    }
}
