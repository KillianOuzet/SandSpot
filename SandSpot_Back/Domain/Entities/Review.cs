namespace Domain.Entities;

public class Review
{
    public int IdReview { get; set; }
    public int Note { get; set; }
    public string? Comment { get; set; }
    public DateTime DateReview { get; set; } = DateTime.UtcNow;

    public int IdUser { get; set; }
    public User User { get; set; } = null!;

    public int IdZone { get; set; }
    public Zone Zone { get; set; } = null!;
}