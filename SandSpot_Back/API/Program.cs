using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Authentication;
using Infrastructure.Repositories;
using Application.Interfaces;
using Application.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        o => o.UseNetTopologySuite() // <--- Active PostGIS pour EF Core
    ));
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// Injection de dépendance pour relier le métier de l'application
// // avec l'utilisation des librairies de l'Infrastrucure
builder.Services.AddScoped<IZoneRepository,ZoneRepository>();
builder.Services.AddScoped<MetierZone>();
builder.Services.AddHttpClient<ZoneImportService>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Vérifie les tokens entrants
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "SandSpotApi",
            ValidAudience = "SandSpotApp",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"] 
                                       ?? throw new InvalidOperationException("Secret manquant.")))
        };
    });

builder.Services.AddAuthorization();

// Autoriser Angular à communiquer avec l'API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDevClient", b =>
    {
        b.WithOrigins("http://localhost:4200") // L'URL du front
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});



var app = builder.Build();

// --- VÉRIFICATION DE LA CONNEXION À LA BASE DE DONNÉES ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        // Import des données des terrains de beach au lancement de l'application ( a enlever plus tard)
        // Remplace IZoneImportService ou ZoneImportService selon le nom de ton service
        var importService = services.GetRequiredService<ZoneImportService>();
        
        logger.LogInformation("⏳ Lancement de l'import des terrains de beach-volley...");
        int count = await importService.ImportZonesFromOpenDataAsync();
        logger.LogInformation("✅ Import terminé : {Count} terrains ajoutés en BDD !", count);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "❌ Erreur lors de l'importation OpenData.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularDevClient");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();