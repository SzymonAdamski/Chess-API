using Microsoft.EntityFrameworkCore;
using Online_Chess_API.Core.Models;

namespace Online_Chess_API.Infrastructure.Data
{
    public class ChessDbContext : DbContext
    {
    // Mapowanie DbSet na tabelę chess_games w SQL Server
    public DbSet<ChessGame> ChessGames { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }

        public ChessDbContext(DbContextOptions<ChessDbContext> options) 
            : base(options)
        {
        }
        
        // Konfiguracja jest dostarczana przez Program.cs, więc nie musimy jej tutaj nadpisywać
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Metoda pusta, konfiguracja dostarczana z zewnątrz
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Konfiguracja encji ChessGame
            modelBuilder.Entity<ChessGame>(entity =>
            {
                // Jawne zmapowanie DbSet ChessGames na tabele chess_games w SQL Server
                entity.ToTable("chess_games", schema: "dbo");
                entity.HasKey(e => e.GameId);
                
                entity.Property(e => e.GameId)
                    .HasColumnName("game_id")
                    .ValueGeneratedOnAdd();
                
                // Konfiguracja relacji z Comment
                entity.HasMany<Comment>()
                    .WithOne(c => c.Game)
                    .HasForeignKey(c => c.GameId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                // Dodanie przykładowych danych do bazy
                entity.HasData(
                    new ChessGame
                    {
                        GameId = "1",
                        WhiteId = "bourgris",
                        BlackId = "a-00",
                        WhiteRating = (short)1500,
                        BlackRating = (short)1191,
                        Rated = false,
                        Turns = (short)13,
                        VictoryStatus = "Out of Time",
                        Winner = "White",
                        TimeIncrement = "15+2",
                        Moves = "d4 d5 c4 c6 cxd5 e6 dxe6 fxe6 Nf3 Bb4+ Nc3 Ba5 Bf4",
                        OpeningCode = "D10",
                        OpeningMoves = (byte)5,
                        OpeningFullname = "Slav Defense: Exchange Variation",
                        OpeningShortname = "Slav Defense"
                    },
                    new ChessGame
                    {
                        GameId = "3",
                        WhiteId = "ischia",
                        BlackId = "a-00",
                        WhiteRating = (short)1496,
                        BlackRating = (short)1500,
                        Rated = true,
                        Turns = (short)61,
                        VictoryStatus = "Mate",
                        Winner = "White",
                        TimeIncrement = "5+10",
                        Moves = "e4 e5 d3 d6 Be3 c6 Be2 b5 Nd2 a5 a4 c5 axb5 Nc6 bxc6 Ra6 Nc4 a4 c3 a3 Nxa3 Rxa3 Rxa3 c4 dxc4 d5 cxd5 Qxd5 exd5 Be6 Ra8+ Ke7 Bc5+ Kf6 Bxf8 Kg6 Bxg7 Kxg7 dxe6 Kh6 exf7 Nf6 Rxh8 Nh5 Bxh5 Kg5 Rxh7 Kf5 Qf3+ Ke6 Bg4+ Kd6 Rh6+ Kc5 Qe3+ Kb5 c4+ Kb4 Qc3+ Ka4 Bd1#",
                        OpeningCode = "C20",
                        OpeningMoves = (byte)3,
                        OpeningFullname = "King's Pawn Game: Leonardis Variation",
                        OpeningShortname = "King's Pawn Game"
                    },
                    new ChessGame
                    {
                        GameId = "4",
                        WhiteId = "daniamurashov",
                        BlackId = "adivanov2009",
                        WhiteRating = (short)1439,
                        BlackRating = (short)1454,
                        Rated = true,
                        Turns = (short)61,
                        VictoryStatus = "Mate",
                        Winner = "White",
                        TimeIncrement = "20+0",
                        Moves = "d4 d5 Nf3 Bf5 Nc3 Nf6 Bf4 Ng4 e3 Nc6 Be2 Qd7 O-O O-O-O Nb5 Nb4 Rc1 Nxa2 Ra1 Nb4 Nxa7+ Kb8 Nb5 Bxc2 Bxc7+ Kc8 Qd2 Qc6 Na7+ Kd7 Nxc6 bxc6 Bxd8 Kxd8 Qxb4 e5 Qb8+ Ke7 dxe5 Be4 Ra7+ Ke6 Qe8+ Kf5 Qxf7+ Nf6 Nh4+ Kg5 g3 Ng4 Qf4+ Kh5 Qxg4+ Kh6 Qf4+ g5 Qf6+ Bg6 Nxg6 Bg7 Qxg7#",
                        OpeningCode = "D02",
                        OpeningMoves = (byte)3,
                        OpeningFullname = "Queen's Pawn Game: Zukertort Variation",
                        OpeningShortname = "Queen's Pawn Game"
                    }
                );
            });
            
            // Konfiguracja encji User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id)
                    .HasColumnName("user_id")
                    .ValueGeneratedOnAdd();
                
                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);
                    
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);
                    
                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(255);
                
                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("datetime('now')");
                    
                // Konfiguracja relacji z Comment
                entity.HasMany<Comment>()
                    .WithOne(c => c.User)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
            
            // Konfiguracja encji Comment
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comments");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
                
                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(1000);
                
                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("datetime('now')");
                
                // Konfiguracja kluczy obcych
                entity.HasOne(c => c.Game)
                    .WithMany(g => g.Comments)
                    .HasForeignKey(c => c.GameId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(c => c.User)
                    .WithMany(u => u.Comments)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
