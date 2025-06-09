using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Online_Chess_API.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixCommentGameIdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_ChessGames_GameId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChessGames",
                table: "ChessGames");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "ChessGames",
                newName: "chess_games",
                newSchema: "dbo");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastLogin",
                table: "Users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "datetime('now')",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Comments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GameId",
                table: "Comments",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "datetime('now')",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Comments",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Comments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "winner",
                schema: "dbo",
                table: "chess_games",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<int>(
                name: "white_rating",
                schema: "dbo",
                table: "chess_games",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "white_id",
                schema: "dbo",
                table: "chess_games",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "victory_status",
                schema: "dbo",
                table: "chess_games",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<int>(
                name: "turns",
                schema: "dbo",
                table: "chess_games",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "time_increment",
                schema: "dbo",
                table: "chess_games",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 7);

            migrationBuilder.AlterColumn<bool>(
                name: "rated",
                schema: "dbo",
                table: "chess_games",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<string>(
                name: "opening_variation",
                schema: "dbo",
                table: "chess_games",
                type: "nvarchar(47)",
                maxLength: 47,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 47,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "opening_shortname",
                schema: "dbo",
                table: "chess_games",
                type: "nvarchar(26)",
                maxLength: 26,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 26);

            migrationBuilder.AlterColumn<string>(
                name: "opening_response",
                schema: "dbo",
                table: "chess_games",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "opening_moves",
                schema: "dbo",
                table: "chess_games",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "opening_fullname",
                schema: "dbo",
                table: "chess_games",
                type: "nvarchar(91)",
                maxLength: 91,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 91);

            migrationBuilder.AlterColumn<string>(
                name: "opening_code",
                schema: "dbo",
                table: "chess_games",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<string>(
                name: "moves",
                schema: "dbo",
                table: "chess_games",
                type: "nvarchar(1413)",
                maxLength: 1413,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 1413);

            migrationBuilder.AlterColumn<int>(
                name: "black_rating",
                schema: "dbo",
                table: "chess_games",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "black_id",
                schema: "dbo",
                table: "chess_games",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "game_id",
                schema: "dbo",
                table: "chess_games",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddPrimaryKey(
                name: "PK_chess_games",
                schema: "dbo",
                table: "chess_games",
                column: "game_id");

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "chess_games",
                columns: new[] { "game_id", "black_id", "black_rating", "moves", "opening_code", "opening_fullname", "opening_moves", "opening_response", "opening_shortname", "opening_variation", "rated", "time_increment", "turns", "victory_status", "white_id", "white_rating", "winner" },
                values: new object[,]
                {
                    { "1", "a-00", 1191, "d4 d5 c4 c6 cxd5 e6 dxe6 fxe6 Nf3 Bb4+ Nc3 Ba5 Bf4", "D10", "Slav Defense: Exchange Variation", 5, null, "Slav Defense", null, false, "15+2", 13, "Out of Time", "bourgris", 1500, "White" },
                    { "3", "a-00", 1500, "e4 e5 d3 d6 Be3 c6 Be2 b5 Nd2 a5 a4 c5 axb5 Nc6 bxc6 Ra6 Nc4 a4 c3 a3 Nxa3 Rxa3 Rxa3 c4 dxc4 d5 cxd5 Qxd5 exd5 Be6 Ra8+ Ke7 Bc5+ Kf6 Bxf8 Kg6 Bxg7 Kxg7 dxe6 Kh6 exf7 Nf6 Rxh8 Nh5 Bxh5 Kg5 Rxh7 Kf5 Qf3+ Ke6 Bg4+ Kd6 Rh6+ Kc5 Qe3+ Kb5 c4+ Kb4 Qc3+ Ka4 Bd1#", "C20", "King's Pawn Game: Leonardis Variation", 3, null, "King's Pawn Game", null, true, "5+10", 61, "Mate", "ischia", 1496, "White" },
                    { "4", "adivanov2009", 1454, "d4 d5 Nf3 Bf5 Nc3 Nf6 Bf4 Ng4 e3 Nc6 Be2 Qd7 O-O O-O-O Nb5 Nb4 Rc1 Nxa2 Ra1 Nb4 Nxa7+ Kb8 Nb5 Bxc2 Bxc7+ Kc8 Qd2 Qc6 Na7+ Kd7 Nxc6 bxc6 Bxd8 Kxd8 Qxb4 e5 Qb8+ Ke7 dxe5 Be4 Ra7+ Ke6 Qe8+ Kf5 Qxf7+ Nf6 Nh4+ Kg5 g3 Ng4 Qf4+ Kh5 Qxg4+ Kh6 Qf4+ g5 Qf6+ Bg6 Nxg6 Bg7 Qxg7#", "D02", "Queen's Pawn Game: Zukertort Variation", 3, null, "Queen's Pawn Game", null, true, "20+0", 61, "Mate", "daniamurashov", 1439, "White" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_chess_games_GameId",
                table: "Comments",
                column: "GameId",
                principalSchema: "dbo",
                principalTable: "chess_games",
                principalColumn: "game_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_chess_games_GameId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_chess_games",
                schema: "dbo",
                table: "chess_games");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "chess_games",
                keyColumn: "game_id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "chess_games",
                keyColumn: "game_id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "chess_games",
                keyColumn: "game_id",
                keyValue: "4");

            migrationBuilder.RenameTable(
                name: "chess_games",
                schema: "dbo",
                newName: "ChessGames");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "LastLogin",
                table: "Users",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedAt",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "datetime('now')");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Comments",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedAt",
                table: "Comments",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "Comments",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedAt",
                table: "Comments",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "datetime('now')");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Comments",
                type: "TEXT",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Comments",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "winner",
                table: "ChessGames",
                type: "TEXT",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<int>(
                name: "white_rating",
                table: "ChessGames",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "white_id",
                table: "ChessGames",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "victory_status",
                table: "ChessGames",
                type: "TEXT",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<int>(
                name: "turns",
                table: "ChessGames",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "time_increment",
                table: "ChessGames",
                type: "TEXT",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(7)",
                oldMaxLength: 7);

            migrationBuilder.AlterColumn<string>(
                name: "rated",
                table: "ChessGames",
                type: "TEXT",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "opening_variation",
                table: "ChessGames",
                type: "TEXT",
                maxLength: 47,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(47)",
                oldMaxLength: 47,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "opening_shortname",
                table: "ChessGames",
                type: "TEXT",
                maxLength: 26,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(26)",
                oldMaxLength: 26);

            migrationBuilder.AlterColumn<string>(
                name: "opening_response",
                table: "ChessGames",
                type: "TEXT",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "opening_moves",
                table: "ChessGames",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "opening_fullname",
                table: "ChessGames",
                type: "TEXT",
                maxLength: 91,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(91)",
                oldMaxLength: 91);

            migrationBuilder.AlterColumn<string>(
                name: "opening_code",
                table: "ChessGames",
                type: "TEXT",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<string>(
                name: "moves",
                table: "ChessGames",
                type: "TEXT",
                maxLength: 1413,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1413)",
                oldMaxLength: 1413);

            migrationBuilder.AlterColumn<int>(
                name: "black_rating",
                table: "ChessGames",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "black_id",
                table: "ChessGames",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "game_id",
                table: "ChessGames",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChessGames",
                table: "ChessGames",
                column: "game_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_ChessGames_GameId",
                table: "Comments",
                column: "GameId",
                principalTable: "ChessGames",
                principalColumn: "game_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
