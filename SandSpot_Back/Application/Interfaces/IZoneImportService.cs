namespace Application.Interfaces;

public interface IZoneImportService
{
    /// <summary>
    /// Récupère les terrains de beach-volley depuis l'API OpenData et les insère dans la base si non existants.
    /// </summary>
    /// <returns>Le nombre de zones ajoutées.</returns>

    Task<int> ImportZonesFromOpenDataAsync(CancellationToken cancellationToken = default);
}