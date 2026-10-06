using System.Net.Http.Json;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using Domain.Entities;
namespace Application.Services;

public class ZoneImportService
{
    private readonly HttpClient _httpClient;
    private readonly IZoneRepository _zoneRepository;
    private readonly ILogger<ZoneImportService> _logger;

    public ZoneImportService(HttpClient httpClient, IZoneRepository zoneRepository, ILogger<ZoneImportService> logger)
    {
        _httpClient = httpClient;
        _zoneRepository = zoneRepository;
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
            var url = $"https://equipements.sports.gouv.fr/api/explore/v2.1/catalog/datasets/data-es/records/?lang=fr&limit={limit}&offset={offset}&where=equip_type_name%3D%22Terrain+de+beach-volley%22";

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

            foreach (var item in response.Results)
            {
                if (item.Coordonnees == null || string.IsNullOrWhiteSpace(item.Nom))
                    continue;

                var point = new Point(item.Coordonnees.Longitude, item.Coordonnees.Latitude) { SRID = 4326 };

                bool exists = await _zoneRepository.ExistsAsync(point, cancellationToken);
                if (!exists)
                {
                    var zone = new Zone
                    {
                        Name = item.Nom ?? "Terrain de beach",
                        Location = point,
                        Address = !string.IsNullOrWhiteSpace(item.Adresse) ? item.Adresse : $"{item.Nom}, {item.Commune}",
                        City = item.Commune ?? "Inconnue",
                        PostalCode = item.CodePostal ?? "00000"
                    };

                    _zoneRepository.AddZone(zone);
                    totalAdded++;
                }
            }

            // 💾 Un seul appel SQL pour enregistrer tout le lot de 100 !
            await _zoneRepository.SaveChangesAsync(cancellationToken);

            // --- GESTION DE LA PAGINATION (DOIT ÊTRE HORS DU FOREACH) ---
            if (response.Results.Count < limit)
            {
                hasMore = false;
            }
            else
            {
                offset += limit; // Passe au lot de 100 suivant
            }
        }

        _logger.LogInformation("🎉 Importation terminée : {Total} nouveaux terrains de beach-volley importés !", totalAdded);
        return totalAdded;
    }
}