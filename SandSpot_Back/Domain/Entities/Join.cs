namespace Domain.Entities;

public class Join
{
    public int UserId { get; set; }
    public int AlertId { get; set; }
    
    public User User { get; set; } = null!;
    public Alert Alert { get; set; } = null!;
}