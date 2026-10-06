using Application.DTOs;
using NetTopologySuite.Geometries;
using Domain.Entities;

namespace Application.Interfaces;

public interface IZoneRepository
{

    Task<List<ZoneDto>> GetAll(Point? point = null);
    
    /// <summary>
    /// Renvoie les zones de la base de données qui sont dans le périmètre donné
    /// </summary>
    /// <param name="point"></param>
    /// <param name="distance"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<ZoneDto>> GetNearbyZones(Point point,double distance,CancellationToken cancellationToken = default);
    
    Task<bool> ExistsAsync(Point location, CancellationToken cancellationToken = default);
    void AddZone(Zone zone);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}