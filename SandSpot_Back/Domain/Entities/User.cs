namespace Domain.Entities;

public class User
{
    public int IdUser { get; set; }
    public string Mail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    
    public int IdRole { get; set; }
    public Role Role { get; set; } = null!;

    public ICollection<Zone> Zones { get; set; } = new List<Zone>();
    public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Like> Likes { get; set; } = new List<Like>();
}