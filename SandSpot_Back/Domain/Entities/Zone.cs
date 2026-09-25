namespace Domain.Entities;

public class Zone
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Address { get; set; } = string.Empty;
    public string City { get; set; }  = string.Empty;
    public string PostalCode { get; set; }   = string.Empty;
    
    public int IdUser { get; set; }
    public User User { get; set; } = null!;
    
    public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
    public ICollection<Review> ReceivedReviews { get; set; } = new List<Review>();
}