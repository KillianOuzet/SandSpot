using Application.DTOs;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Application.Services;

public class MetierZone
{
    private readonly ApplicationDbContext _context;

    public MetierZone(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ZoneDto>> GetZonesNearbyAsync(double latitude, double longitude, double radiusInKm, CancellationToken cancellationToken = default)
    {
        // Point recherché (Longitude X, Latitude Y)
        var userPoint = new Point(longitude, latitude) { SRID = 4326 };

        // PostGIS mesure la distance géographique (IsWithinDistance / Distance) en degrés WGS84
        // Approximation standard : 1 degré ~ 111,32 km
        double radiusInDegrees = radiusInKm / 111.32;

        // 1. Exécution de la requête SQL PostGIS (sans Math.Round)
        var rawZones = await _context.Zones 
            .Where(z => z.Location.IsWithinDistance(userPoint, radiusInDegrees))
            .Select(z => new
            {
                z.Id,
                z.Name,
                z.Location,
                z.Address,
                z.City,
                z.PostalCode,
                // Calcul de la distance brute en km sans arrondi SQL
                RawDistanceInKm = z.Location.Distance(userPoint) * 111.32
            })
            .OrderBy(z => z.RawDistanceInKm) // Tri sur la valeur numérique brute
            .ToListAsync(cancellationToken);

        // 2. Projection et arrondi en mémoire (C#)
        return rawZones.Select(z => new ZoneDto
        {
            Id = z.Id,
            Name = z.Name,
            Latitude = z.Location.Y,  // Y = Latitude
            Longitude = z.Location.X, // X = Longitude
            Address = z.Address,
            City = z.City,
            PostalCode = z.PostalCode,
            DistanceInKm = Math.Round(z.RawDistanceInKm, 2) // Arrondi exécuté côté C#
        }).ToList();
    }
}