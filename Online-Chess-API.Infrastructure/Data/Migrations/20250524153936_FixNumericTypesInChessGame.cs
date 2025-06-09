using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Chess_API.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixNumericTypesInChessGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "white_rating",
                schema: "dbo",
                table: "chess_games",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<short>(
                name: "turns",
                schema: "dbo",
                table: "chess_games",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte>(
                name: "opening_moves",
                schema: "dbo",
                table: "chess_games",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<short>(
                name: "black_rating",
                schema: "dbo",
                table: "chess_games",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "chess_games",
                keyColumn: "game_id",
                keyValue: "1",
                columns: new[] { "black_rating", "opening_moves", "turns", "white_rating" },
                values: new object[] { (short)1191, (byte)5, (short)13, (short)1500 });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "chess_games",
                keyColumn: "game_id",
                keyValue: "3",
                columns: new[] { "black_rating", "opening_moves", "turns", "white_rating" },
                values: new object[] { (short)1500, (byte)3, (short)61, (short)1496 });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "chess_games",
                keyColumn: "game_id",
                keyValue: "4",
                columns: new[] { "black_rating", "opening_moves", "turns", "white_rating" },
                values: new object[] { (short)1454, (byte)3, (short)61, (short)1439 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "white_rating",
                schema: "dbo",
                table: "chess_games",
                type: "int",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<int>(
                name: "turns",
                schema: "dbo",
                table: "chess_games",
                type: "int",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<int>(
                name: "opening_moves",
                schema: "dbo",
                table: "chess_games",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<int>(
                name: "black_rating",
                schema: "dbo",
                table: "chess_games",
                type: "int",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "chess_games",
                keyColumn: "game_id",
                keyValue: "1",
                columns: new[] { "black_rating", "opening_moves", "turns", "white_rating" },
                values: new object[] { 1191, 5, 13, 1500 });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "chess_games",
                keyColumn: "game_id",
                keyValue: "3",
                columns: new[] { "black_rating", "opening_moves", "turns", "white_rating" },
                values: new object[] { 1500, 3, 61, 1496 });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "chess_games",
                keyColumn: "game_id",
                keyValue: "4",
                columns: new[] { "black_rating", "opening_moves", "turns", "white_rating" },
                values: new object[] { 1454, 3, 61, 1439 });
        }
    }
}
