using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Chess_API.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChessGames",
                columns: table => new
                {
                    game_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    rated = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    turns = table.Column<int>(type: "INTEGER", nullable: false),
                    victory_status = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false),
                    winner = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    time_increment = table.Column<string>(type: "TEXT", maxLength: 7, nullable: false),
                    white_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    white_rating = table.Column<int>(type: "INTEGER", nullable: false),
                    black_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    black_rating = table.Column<int>(type: "INTEGER", nullable: false),
                    moves = table.Column<string>(type: "TEXT", maxLength: 1413, nullable: false),
                    opening_code = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    opening_moves = table.Column<int>(type: "INTEGER", nullable: false),
                    opening_fullname = table.Column<string>(type: "TEXT", maxLength: 91, nullable: false),
                    opening_shortname = table.Column<string>(type: "TEXT", maxLength: 26, nullable: false),
                    opening_response = table.Column<string>(type: "TEXT", maxLength: 8, nullable: true),
                    opening_variation = table.Column<string>(type: "TEXT", maxLength: 47, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChessGames", x => x.game_id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastLogin = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_ChessGames_GameId",
                        column: x => x.GameId,
                        principalTable: "ChessGames",
                        principalColumn: "game_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_GameId",
                table: "Comments",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "ChessGames");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
