namespace Domain.Entities;

public class Alert
{
    public int IdAlert { get; set; }
    public DateTime DateAlert { get; set; }
    public string? Description { get; set; }
    public bool Ball { get; set; }
    public bool Net { get; set; }

    public int IdUser { get; set; }
    public User User { get; set; } = null!;

    public int IdZone { get; set; }
    public Zone Zone { get; set; } = null!;

    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public ICollection<Like> Likes { get; set; } = new List<Like>();
}