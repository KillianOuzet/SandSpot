using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        // Chercher un utilisateur par son email (retourne null s'il n'existe pas)
        Task<User?> GetByEmailAsync(string email);
        
        // Chercher un rôle (pour attribuer le rôle "Joueur" par défaut)
        Task<Role?> GetRoleByNameAsync(string roleName);
        
        // Ajouter un nouvel utilisateur en base
        Task AddAsync(User user);
    }
}