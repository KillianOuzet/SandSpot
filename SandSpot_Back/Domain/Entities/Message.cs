namespace Domain.Entities;

public class Message
{
    public int Id { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public string Content { get; set; } = string.Empty;

    public int SenderId { get; set; }
    public int AlertId { get; set; }
    public User Sender { get; set; } = null!;
    public Alert Alert { get; set; } = null!;
}