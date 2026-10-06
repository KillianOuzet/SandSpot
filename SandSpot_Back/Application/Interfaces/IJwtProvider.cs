using Domain.Entities;

namespace Application.Interfaces
{
    public interface IJwtProvider
    {
        // Génère le token qui servira de passeport à l'utilisateur
        string Generate(User user);
    }
}