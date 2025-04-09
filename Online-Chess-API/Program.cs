using Microsoft.EntityFrameworkCore;
using Online_Chess_API.Core.DTOs;
using Online_Chess_API.Core.Interfaces;
using Online_Chess_API.Core.Models;
using Online_Chess_API.Infrastructure.Data;
using Online_Chess_API.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Rejestracja DbContext
builder.Services.AddDbContext<ChessDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Rejestracja repozytoriów
builder.Services.AddScoped<IChessGameRepository, ChessGameRepository>();
// Dodaj inne repozytoria jeśli są potrzebne
// builder.Services.AddScoped<ICommentRepository, CommentRepository>();
// builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Zachowaj istniejący endpoint weatherforecast
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

// Dodaj endpointy dla ChessGame

// GET: Pobierz wszystkie gry z paginacją
app.MapGet("/chess-games", async (IChessGameRepository repo, [AsParameters] PaginationDto pagination) =>
{
    var games = await repo.GetAllGamesAsync(pagination);
    var count = await repo.GetTotalGamesCountAsync();
    return new PagedResponseDto<ChessGame>
    {
        Items = games.ToList(),
        PageNumber = pagination.PageNumber,
        PageSize = pagination.PageSize
    };
})
.WithName("GetAllChessGames")
.WithOpenApi();

// GET: Pobierz grę po ID
app.MapGet("/chess-games/{id}", async (IChessGameRepository repo, int id) =>
{
    var game = await repo.GetGameByIdAsync(id);
    return game is null ? Results.NotFound() : Results.Ok(game);
})
.WithName("GetChessGameById")
.WithOpenApi();

// POST: Utwórz nową grę
app.MapPost("/chess-games", async (IChessGameRepository repo, ChessGame game) =>
{
    var createdGame = await repo.CreateGameAsync(game);
    return Results.Created($"/chess-games/{createdGame.GameId}", createdGame);
})
.WithName("CreateChessGame")
.WithOpenApi();

// PUT: Aktualizuj grę
app.MapPut("/chess-games/{id}", async (IChessGameRepository repo, int id, ChessGame game) =>
{
    if (id != game.GameId)
        return Results.BadRequest();
        
    var updatedGame = await repo.UpdateGameAsync(game);
    return Results.Ok(updatedGame);
})
.WithName("UpdateChessGame")
.WithOpenApi();

// DELETE: Usuń grę
app.MapDelete("/chess-games/{id}", async (IChessGameRepository repo, int id) =>
{
    var result = await repo.DeleteGameAsync(id);
    return result ? Results.NoContent() : Results.NotFound();
})
.WithName("DeleteChessGame")
.WithOpenApi();

// GET: Filtruj gry
app.MapGet("/chess-games/filter", async (IChessGameRepository repo, string property, string value, [AsParameters] PaginationDto pagination) =>
{
    var games = await repo.FilterGamesAsync(property, value, pagination);
    var count = await repo.GetTotalGamesCountAsync(); // To można zoptymalizować, dodając metodę do liczenia przefiltrowanych wyników
    return new PagedResponseDto<ChessGame>
    {
        Items = games.ToList(),
        PageNumber = pagination.PageNumber,
        PageSize = pagination.PageSize
    };
})
.WithName("FilterChessGames")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}