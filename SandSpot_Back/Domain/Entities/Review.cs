namespace Domain.Entities;

public class Review
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int AuthorId { get; set; }
    public int ZoneId { get; set; }
    
    public User Author { get; set; } = null!;
    public Zone Zone { get; set; } = null!;
}