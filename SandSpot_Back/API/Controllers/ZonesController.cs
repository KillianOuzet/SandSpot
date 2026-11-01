using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Services;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ZonesController : ControllerBase
{
    private readonly MetierZone _metierZone;

    public ZonesController(MetierZone metierZone)
    {
        _metierZone = metierZone;
    }
    
    public async Task<ActionResult<List<ZoneDto>>> GetAllZones(
        [FromQuery] double? latitude,
        [FromQuery] double? longitude)
    {
        var zones = await _metierZone.GetAllZonesAsync(latitude, longitude);
        return Ok(zones);
    }
    
    /// <summary>
    /// Récupère les terrains de beach-volley situés dans un rayon géometrique autour d'un point GPS.
    /// Ex: GET /api/zones/nearby?latitude=46.1591&longitude=-1.1517&radiusInKm=20
    /// </summary>
    [HttpGet("nearby")]
    public async Task<ActionResult<List<ZoneDto>>> GetNearbyZones(
        [FromQuery] double latitude,
        [FromQuery] double longitude,
        [FromQuery] double radiusInKm = 10.0) // 10 km par défaut si non spécifié
    {
        if (radiusInKm <= 0 || radiusInKm > 500)
        {
            return BadRequest("Le rayon doit être compris entre 0 et 500 km.");
        }

        var zones = await _metierZone.GetZonesNearbyAsync(latitude, longitude, radiusInKm);
        return Ok(zones);
    }
}