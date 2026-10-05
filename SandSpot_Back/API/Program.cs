using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Application.Services;

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

// Injection du service d'import d'OpenData
builder.Services.AddHttpClient<IZoneImportService, ZoneImportService>();
builder.Services.AddScoped<MetierZone>();



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
        var importService = services.GetRequiredService<IZoneImportService>();
        
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

app.UseAuthorization();

app.MapControllers();

app.Run();