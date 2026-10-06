using Application.DTOs;
using Application.Interfaces;
using NetTopologySuite.Geometries;

namespace Application.Services;

public class MetierZone
{
    private IZoneRepository _zoneRepository;

    public MetierZone(IZoneRepository zoneRepository)
    {
        _zoneRepository = zoneRepository;
    }

    public async Task<List<ZoneDto>> GetZonesNearbyAsync(double latitude, double longitude, double radiusInKm, CancellationToken cancellationToken = default)
    {
        // Point recherché (Longitude X, Latitude Y)
        var userPoint = new Point(longitude, latitude) { SRID = 4326 };

        // PostGIS mesure la distance géographique (IsWithinDistance / Distance) en degrés WGS84
        // Approximation standard : 1 degré ~ 111,32 km
        double radiusInDegrees = radiusInKm / 111.32;

        // 1. Exécution de la requête SQL PostGIS (sans Math.Round)
        return await _zoneRepository.GetNearbyZones(userPoint, radiusInDegrees, cancellationToken);
    }
}