using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();



var app = builder.Build();

// --- VÉRIFICATION DE LA CONNEXION À LA BASE DE DONNÉES ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        // Teste si la connexion réseau/identifiants fonctionne
        if (context.Database.CanConnect())
        {
            logger.LogInformation("✅ Connexion à la base de données PostgreSQL réussie !");
        }
        else
        {
            logger.LogError("❌ Impossible de se connecter à la base de données.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "❌ Erreur lors de la connexion à la base de données.");
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