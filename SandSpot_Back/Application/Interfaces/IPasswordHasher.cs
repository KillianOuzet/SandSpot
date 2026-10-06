namespace Application.Interfaces
{
    public interface IPasswordHasher
    {
        // Transforme le mot de passe en clair en une empreinte sécurisée
        string Hash(string password);
        
        // Vérifie si le mot de passe tapé à la connexion correspond à l'empreinte en BDD
        bool Verify(string password, string passwordHash);
    }
}