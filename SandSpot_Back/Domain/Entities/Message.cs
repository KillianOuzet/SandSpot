namespace Domain.Entities;

public class Message
{
    public int IdMessage { get; set; }
    public DateTime DateMessage { get; set; } = DateTime.UtcNow;
    public string Content { get; set; } = string.Empty;

    public int IdUser { get; set; }
    public User User { get; set; } = null!;

    public int IdAlert { get; set; }
    public Alert Alert { get; set; } = null!;
}