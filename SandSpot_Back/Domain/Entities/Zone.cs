namespace Domain.Entities;
using NetTopologySuite.Geometries;

public class Zone
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Point Location { get; set; } = null!; // Représente un Point(Longitude, Latitude)
    public string Address { get; set; } = string.Empty;
    public string City { get; set; }  = string.Empty;
    public string PostalCode { get; set; }   = string.Empty;
    
    public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
    public ICollection<Review> ReceivedReviews { get; set; } = new List<Review>();
}