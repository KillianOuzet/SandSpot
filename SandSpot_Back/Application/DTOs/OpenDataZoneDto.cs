using System.Text.Json.Serialization;

namespace Application.DTOs;

public class OpenDataZoneDto
{
    // L'API v2.1 utilise "results" (ou parfois "records")
    [JsonPropertyName("results")]
    public List<OpenDataResult> Results { get; set; } = new();
}

public class OpenDataResult
{
    // Nom de l'équipement au premier niveau
    [JsonPropertyName("inst_nom")]
    public string? Nom { get; set; }

    [JsonPropertyName("new_name")]
    public string? Commune { get; set; }

    [JsonPropertyName("inst_cp")]
    public string? CodePostal { get; set; }

    [JsonPropertyName("inst_adresse")]
    public string? Adresse { get; set; }

    // Les coordonnées géographiques (coordonnees.lat, coordonnees.lon)
    [JsonPropertyName("equip_coordonnees")]
    public OpenDataCoordonnees? Coordonnees { get; set; }
}

public class OpenDataCoordonnees
{
    [JsonPropertyName("lon")]
    public double Longitude { get; set; }

    [JsonPropertyName("lat")]
    public double Latitude { get; set; }
}