namespace Domain.Entities;

public class Alert
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public string? Description { get; set; }
    public int MaxPlayers { get; set; }
    public bool Ball { get; set; }
    public bool Net { get; set; }

    public int CreatorId { get; set; }
    public int ZoneId { get; set; }
    public int LevelId { get; set; }
    public User Creator { get; set; } = null!;
    public Zone Zone { get; set; } = null!;
    public Level Level { get; set; } = null!;

    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public ICollection<Join> Participants { get; set; } = new List<Join>();
}