namespace Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Mail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
    
    public ICollection<Alert> CreatedAlerts { get; set; } = new List<Alert>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public ICollection<Review> AuthoredReviews { get; set; } = new List<Review>();
    public ICollection<Join> JoinedAlerts { get; set; } = new List<Join>();
}