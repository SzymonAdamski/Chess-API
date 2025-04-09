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
                    GameId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rated = table.Column<bool>(type: "bit", nullable: false),
                    Turns = table.Column<int>(type: "int", nullable: false),
                    VictoryStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Winner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeIncrement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhiteId = table.Column<int>(type: "int", nullable: false),
                    WhiteRating = table.Column<int>(type: "int", nullable: false),
                    BlackId = table.Column<int>(type: "int", nullable: false),
                    BlackRating = table.Column<int>(type: "int", nullable: false),
                    Moves = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningMoves = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningFullname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningShortname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningResponse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningVariation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChessGames", x => x.GameId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_ChessGames_GameId",
                        column: x => x.GameId,
                        principalTable: "ChessGames",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
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
