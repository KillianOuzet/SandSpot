namespace Application.Interfaces
{
    public interface IAuthService
    {
        // Retourne le JWT si l'inscription réussit
        Task<string> RegisterAsync(string username, string email, string password);
        
        // Retourne le JWT si la connexion réussit
        Task<string> LoginAsync(string email, string password);
    }
}