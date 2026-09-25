namespace Domain.Entities;

public class Level
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
}