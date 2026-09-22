namespace Domain.Entities;

public class Like
{
    public int IdLike { get; set; }
    public bool HasLiked { get; set; } = true;

    public int IdUser { get; set; }
    public User User { get; set; } = null!;

    public int IdAlert { get; set; }
    public Alert Alert { get; set; } = null!;
}