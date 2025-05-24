using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Online_Chess_API.Core.DTOs;
using Online_Chess_API.Core.Interfaces;
using Online_Chess_API.Core.Models;
using Online_Chess_API.Infrastructure.Data;
using Online_Chess_API.Infrastructure.Repositories;
using Online_Chess_API.Infrastructure.Services;
using System.Text;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Konfiguracja logowania
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dodaj konfigurację serializacji JSON z obsługą cyklicznych referencji
builder.Services.AddControllers()
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// Konfiguracja globalnych opcji serializacji JSON dla Minimal API
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    options.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

// Rejestracja DbContext dla SQL Server
builder.Services.AddDbContext<ChessDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Rejestracja repozytoriów
builder.Services.AddScoped<IChessGameRepository, ChessGameRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Rejestracja serwisów
builder.Services.AddScoped<IAuthService, AuthService>();

// Konfiguracja uwierzytelniania JWT
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
{
    throw new ArgumentNullException("JWT configuration is missing. Please check appsettings.json");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Konfiguracja Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Chess API", Version = "v1" });
    
    // Dodaj obsługę JWT w Swaggerze
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Konfiguracja middleware ze szczegółowym logowaniem
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    
    // Logowanie szczegółów żądania
    logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path} {context.Request.QueryString}");
    
    // Logowanie nagłówków
    foreach (var header in context.Request.Headers)
    {
        logger.LogDebug($"Header: {header.Key}: {header.Value}");
    }
    
    // Wykonanie kolejnego middleware w potoku
    await next();
    
    // Logowanie odpowiedzi z dodatkową informacją o trasie
    logger.LogInformation($"Response: {context.Response.StatusCode} dla żądania {context.Request.Method} {context.Request.Path}");
    
    // Jeśli błąd 404, wypisujemy dostępne trasy dla celów diagnostycznych
    if (context.Response.StatusCode == 404)
    {
        logger.LogWarning($"404 Not Found dla żądania {context.Request.Method} {context.Request.Path} - sprawdź czy trasa istnieje w aplikacji");
        logger.LogWarning($"Sprawdź czy URL jest poprawny, zwróć uwagę na wielkość liter i dodatkowe znaki");
    }
});

// Middleware dla obsługi HTTP i bezpieczeństwa
app.UseHttpsRedirection();

// Middleware dla uwierzytelniania i autoryzacji
app.UseAuthentication();
app.UseAuthorization();

// Swagger tylko w trybie deweloperskim
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Sprawdzenie połączenia z bazą danych
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ChessDbContext>();
        
        // Sprawdź czy baza danych istnieje
        if (await dbContext.Database.CanConnectAsync())
        {
            Console.WriteLine("Połączenie z bazą danych ustanowione pomyślnie.");
            
            // Sprawdź dostępne dane
            var games = await dbContext.ChessGames.ToListAsync();
            Console.WriteLine($"Liczba gier w bazie: {games.Count}");
            
            if (games.Count > 0)
            {
                Console.WriteLine("Przykładowe gry:");
                foreach (var game in games.Take(5))
                {
                    Console.WriteLine($"Game ID: {game.GameId}, WhiteId: {game.WhiteId}, BlackId: {game.BlackId}");
                }
            }
            else
            {
                Console.WriteLine("Brak gier w bazie danych. Zaimportuj dane z pliku chess_games.sql do bazy danych.");
            }
        }
        else
        {
            Console.WriteLine("Nie można połączyć się z bazą danych. Sprawdź connection string w appsettings.json.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Wystąpił błąd podczas inicjalizacji bazy danych: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply migrations and create database if not exists + dodanie przykładowych danych
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<ChessDbContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();
        
        // Upewnij się, że baza danych istnieje
        dbContext.Database.EnsureCreated();
        
        // Sprawdź czy w bazie są już gry szachowe
        if (!dbContext.ChessGames.Any())
        {
            logger.LogInformation("Dodawanie przykładowych gier szachowych do bazy danych...");
            
            // Dodaj przykładowe gry szachowe na podstawie pliku chess_games.sql
            var games = new List<ChessGame>
            {
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
                    Moves = "d4 d5 Nf3 Bf5 Nc3 Nf6 Bf4 Ng4 e3 Nc6 Be2 Qd7 O-O O-O-O Nb5 Nb4 Rc1 Nxa2 Ra1 Nb4 Nxa7+ Kb8 Nb5 Bxc2 Bxc7+ Kc8 Qd2 Qc6 Na7+ Kd7 Nxc6 bxc6 Bxd8 Kxd8 Qxb4 e5 Qb8+ Ke7 dxe5 Be4 Ra7+ Ke6 Qe8+ Kf5 Qxf7+ Nf6 Nh4+ Kg5 g3 Ng4 Qf4+ Kh5 Qxg4+ Kh6 Qf6+ Bg6 Nxg6 Bg7 Qxg7#",
                    OpeningCode = "D02",
                    OpeningMoves = (byte)3,
                    OpeningFullname = "Queen's Pawn Game: Zukertort Variation",
                    OpeningShortname = "Queen's Pawn Game"
                }
            };
            
            // Dodaj gry do bazy danych
            dbContext.ChessGames.AddRange(games);
            dbContext.SaveChanges();
            
            logger.LogInformation($"Dodano {games.Count} przykładowych gier szachowych do bazy danych.");
        }
        else
        {
            var count = dbContext.ChessGames.Count();
            logger.LogInformation($"Baza danych zawiera już {count} gier szachowych.");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or initializing the database.");
    }
}

app.UseHttpsRedirection();

// Dodaj middleware uwierzytelniania i autoryzacji
app.UseAuthentication();
app.UseAuthorization();

// Endpoint testowy dla sprawdzenia czy aplikacja działa poprawnie
app.MapGet("/api/test", () => new { Message = "API działa poprawnie", Timestamp = DateTime.Now })
    .WithName("ApiTest")
    .WithOpenApi();

app.MapGet("/test", () => "Prosty endpoint testowy - działa!")
    .WithName("SimpleTest")
    .WithOpenApi();

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

// Upewniam się, że kolejność endpointów jest poprawna dla prawidłowego routingu

// 1. Zdefiniuj najpierw endpointy z konkretnymi ścieżkami (bez parametrów)

// GET: Pobierz wszystkie gry z paginacją
app.MapGet("/chess-games", async (IChessGameRepository repo, int? pageNumber = 1, int? pageSize = 10, string? sortBy = "GameId", bool? sortDescending = false) =>
{
    var pagination = new PaginationDto
    {
        PageNumber = pageNumber ?? 1,
        PageSize = pageSize ?? 10,
        SortBy = sortBy ?? "GameId",
        SortDescending = sortDescending ?? false
    };
    var games = await repo.GetAllGamesAsync(pagination);
    var count = await repo.GetTotalGamesCountAsync();
    var totalPages = (int)Math.Ceiling(count / (double)pagination.PageSize);
    return new PagedResponseDto<ChessGame>
    {
        Items = games.ToList(),
        PageNumber = pagination.PageNumber,
        PageSize = pagination.PageSize,
        TotalItems = count,
        TotalPages = totalPages,
        HasPreviousPage = pagination.PageNumber > 1,
        HasNextPage = pagination.PageNumber < totalPages
    };
})
.WithName("GetAllChessGames")
.WithOpenApi();

// 2. Zdefiniuj endpointy ze specyficznymi segmentami URL (np. 'filter')

// GET: Filtruj gry - UWAGA: musi być przed endpointem z parametrem {id}
app.MapGet("/chess-games/filter", async (IChessGameRepository repo, string property, string value, int? pageNumber = 1, int? pageSize = 10, string? sortBy = "GameId", bool? sortDescending = false) =>
{
    var pagination = new PaginationDto
    {
        PageNumber = pageNumber ?? 1,
        PageSize = pageSize ?? 10,
        SortBy = sortBy ?? "GameId",
        SortDescending = sortDescending ?? false
    };
    
    var games = await repo.FilterGamesAsync(property, value, pagination);
    var count = await repo.GetTotalGamesCountAsync(); // To można zoptymalizować, dodając metodę do liczenia przefiltrowanych wyników
    var totalPages = (int)Math.Ceiling(count / (double)pagination.PageSize);
    
    return new PagedResponseDto<ChessGame>
    {
        Items = games.ToList(),
        PageNumber = pagination.PageNumber,
        PageSize = pagination.PageSize,
        TotalItems = count,
        TotalPages = totalPages,
        HasPreviousPage = pagination.PageNumber > 1,
        HasNextPage = pagination.PageNumber < totalPages
    };
})
.WithName("FilterChessGames")
.WithOpenApi();

// 3. Na końcu zdefiniuj endpointy z parametrami (np. {id})

// GET: Pobierz grę po ID - format bez ograniczenia :int
app.MapGet("/chess-games/{id}", async (IChessGameRepository repo, string id) =>
{
    try
    {
        var game = await repo.GetGameByIdAsync(id);
        if (game == null)
        {
            Console.WriteLine($"Game with ID {id} not found");
            return Results.NotFound(new { message = $"Game with ID {id} not found" });
        }
        Console.WriteLine($"Successfully retrieved game with ID {id}");
        return Results.Ok(game);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error retrieving game with ID {id}: {ex.Message}");
        return Results.Problem($"Error retrieving game: {ex.Message}");
    }
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
app.MapPut("/chess-games/{id}", async (IChessGameRepository repo, string id, ChessGame game) =>
{
    if (id != game.GameId)
        return Results.BadRequest();
        
    var updatedGame = await repo.UpdateGameAsync(game);
    return Results.Ok(updatedGame);
})
.WithName("UpdateChessGame")
.WithOpenApi();

// DELETE: Usuń grę
app.MapDelete("/chess-games/{id}", async (IChessGameRepository repo, string id) =>
{
    var result = await repo.DeleteGameAsync(id);
    return result ? Results.NoContent() : Results.NotFound();
})
.WithName("DeleteChessGame")
.WithOpenApi();

// Dodaj endpointy dla autentykacji

// POST: Rejestracja nowego użytkownika
app.MapPost("/auth/register", async (IAuthService authService, UserRegisterDto userDto) =>
{
    try
    {
        var user = await authService.RegisterAsync(userDto);
        return Results.Created($"/users/{user.Id}", new { user.Id, user.Username, user.Email, user.CreatedAt });
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(ex.Message);
    }
})
.WithName("RegisterUser")
.WithOpenApi();

// POST: Logowanie użytkownika
app.MapPost("/auth/login", async (IAuthService authService, UserLoginDto userDto) =>
{
    try
    {
        var token = await authService.LoginAsync(userDto);
        return Results.Ok(new { Token = token });
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(ex.Message);
    }
})
.WithName("LoginUser")
.WithOpenApi();

// Dodaj endpointy dla użytkowników

// GET: Pobierz użytkownika po ID
app.MapGet("/users/{id}", async (IUserRepository repo, int id) =>
{
    var user = await repo.GetUserByIdAsync(id);
    return user is null ? Results.NotFound() : Results.Ok(new { user.Id, user.Username, user.Email, user.CreatedAt, user.LastLogin });
})
.WithName("GetUserById")
.WithOpenApi();

// Dodaj endpointy dla komentarzy

// GET: Pobierz komentarze dla gry
app.MapGet("/chess-games/{gameId}/comments", async (ICommentRepository repo, string gameId, int? pageNumber = 1, int? pageSize = 10) =>
{
    var pagination = new PaginationDto
    {
        PageNumber = pageNumber ?? 1,
        PageSize = pageSize ?? 10,
        SortBy = "CreatedAt",
        SortDescending = true
    };
    
    var comments = await repo.GetCommentsByGameIdAsync(gameId, pagination);
    var count = await repo.GetTotalCommentsCountForGameAsync(gameId);
    var totalPages = (int)Math.Ceiling(count / (double)pagination.PageSize);
    
    return Results.Ok(new PagedResponseDto<Comment>
    {
        Items = comments.ToList(),
        PageNumber = pagination.PageNumber,
        PageSize = pagination.PageSize,
        TotalItems = count,
        TotalPages = totalPages,
        HasPreviousPage = pagination.PageNumber > 1,
        HasNextPage = pagination.PageNumber < totalPages
    });
})
.WithName("GetCommentsByGameId")
.WithOpenApi();

// POST: Dodaj komentarz do gry
app.MapPost("/chess-games/{gameId}/comments", async (ICommentRepository repo, string gameId, int userId, Comment comment) =>
{
    comment.GameId = gameId;
    comment.UserId = userId;
    comment.CreatedAt = DateTime.UtcNow;
    
    var createdComment = await repo.CreateCommentAsync(comment);
    return Results.Created($"/chess-games/{gameId}/comments/{createdComment.Id}", createdComment);
})
.WithName("AddCommentToGame")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
