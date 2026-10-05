using System.Net.Http.Json;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;

namespace Application.Services;

public class ZoneImportService : IZoneImportService
{
    private readonly HttpClient _httpClient;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ZoneImportService> _logger;

    public ZoneImportService(HttpClient httpClient, ApplicationDbContext context, ILogger<ZoneImportService> logger)
    {
        _httpClient = httpClient;
        _context = context;
        _logger = logger;
    }

    public async Task<int> ImportZonesFromOpenDataAsync(CancellationToken cancellationToken = default)
{
    int limit = 100;
    int offset = 0;
    int totalAdded = 0;
    bool hasMore = true;

    while (hasMore)
    {
        string url = $"https://equipements.sports.gouv.fr/api/explore/v2.1/catalog/datasets/data-es/records/?lang=fr&limit={limit}&offset={offset}&where=equip_type_name%3D%22Terrain+de+beach-volley%22";

        _logger.LogInformation("⏳ Récupération du lot d'équipements (Offset: {Offset})...", offset);

        OpenDataZoneDto? response = null;
        try
        {
            response = await _httpClient.GetFromJsonAsync<OpenDataZoneDto>(url, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur HTTP lors de la récupération du lot à l'offset {Offset}", offset);
            break;
        }

        if (response?.Results == null || !response.Results.Any())
        {
            hasMore = false;
            break;
        }

        int addedInThisBatch = 0;

        foreach (var item in response.Results)
        {
            if (item.Coordonnees == null || string.IsNullOrWhiteSpace(item.Nom))
                continue;
            
            // Création du Point PostGIS (Attention : Longitude en 1er, Latitude en 2nd)
            var point = new Point(item.Coordonnees.Longitude, item.Coordonnees.Latitude) { SRID = 4326 };

            string name = item.Nom ?? "Terrain de beach";
            string city = item.Commune ?? "Inconnue";
            string postalCode = item.CodePostal ?? "00000";
            string address = !string.IsNullOrWhiteSpace(item.Adresse) ? item.Adresse : $"{name}, {city}";

            // Vérification de doublon en BDD
            bool exists = await _context.Zones.AnyAsync(z => 
                z.Location.IsWithinDistance(point, 0.0001), cancellationToken);
            
            if (!exists)
            {
                var zone = new Zone
                {
                    Name = name,
                    Location = point,
                    Address = address,
                    City = city,
                    PostalCode = postalCode
                };

                _context.Zones.Add(zone);
                addedInThisBatch++;
            }
        }

        if (addedInThisBatch > 0)
        {
            await _context.SaveChangesAsync(cancellationToken);
            totalAdded += addedInThisBatch;
        }

        // Si le lot reçu est inférieur à 'limit', c'est qu'on a atteint la dernière page
        if (response.Results.Count < limit)
        {
            hasMore = false;
        }
        else
        {
            // Passe à la page suivante
            offset += limit;
        }
    }

    _logger.LogInformation("🎉 Importation terminée : {Total} nouveaux terrains de beach-volley importés !", totalAdded);
    return totalAdded;
}
}