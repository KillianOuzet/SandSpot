using Application.Interfaces;

namespace Infrastructure.Authentication
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            // Le WorkFactor définit la "lourdeur" du calcul (11 est un bon standard actuel).
            // Plus il est élevé, plus c'est sécurisé, mais plus ça demande de ressources serveur.
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 11);
        }

        public bool Verify(string password, string passwordHash)
        {
            // Vérifie si le mot de passe en clair correspond à l'empreinte de la BDD
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}