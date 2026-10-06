using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class ZoneRepository : IZoneRepository
{
    private readonly ApplicationDbContext _context;

    public ZoneRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ZoneDto>> GetNearbyZones(Point point, double distance, CancellationToken cancellationToken = default)
    {
        var rawZones = await _context.Zones 
            .Where(z => z.Location.IsWithinDistance(point, distance))
            .Select(z => new
            {
                z.Id,
                z.Name,
                z.Location,
                z.Address,
                z.City,
                z.PostalCode,
                RawDistanceInKm = z.Location.Distance(point) * 111.32
            })
            .OrderBy(z => z.RawDistanceInKm)
            .ToListAsync(cancellationToken);

        return rawZones.Select(z => new ZoneDto
        {
            Id = z.Id,
            Name = z.Name,
            Latitude = z.Location.Y,
            Longitude = z.Location.X,
            Address = z.Address,
            City = z.City,
            PostalCode = z.PostalCode,
            DistanceInKm = Math.Round(z.RawDistanceInKm, 2)
        }).ToList();
    }

    public async Task<bool> ExistsAsync(Point location, CancellationToken cancellationToken = default)
    {
        return await _context.Zones.AnyAsync(z => z.Location.IsWithinDistance(location, 0.0001), cancellationToken);
    }

    public void AddZone(Zone zone)
    {
        _context.Zones.Add(zone);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}