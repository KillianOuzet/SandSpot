namespace Domain.Entities;

public class Zone
{
    public int IdZone { get; set; }
    public string? Coordinates { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    
    public int IdUser { get; set; }
    public User User { get; set; } = null!;
    
    public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}