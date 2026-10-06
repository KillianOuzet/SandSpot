using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public AuthService(
            IUserRepository userRepository, 
            IPasswordHasher passwordHasher, 
            IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<string> RegisterAsync(string username, string email, string password)
        {
            // Vérifier si l'utilisateur existe déjà
            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null)
            {
                throw new Exception("Cet email est déjà utilisé."); 
                // Note : Plus tard, tu pourras utiliser des exceptions personnalisées (ex: BadRequestException)
            }

            // Récupérer le rôle par défaut (Joueur)
            var playerRole = await _userRepository.GetRoleByNameAsync("Joueur");
            if (playerRole == null)
            {
                throw new Exception("Le rôle par défaut est introuvable en base de données.");
            }

            // Hacher le mot de passe
            var hashedPassword = _passwordHasher.Hash(password);

            // Créer l'entité User
            var newUser = new User
            {
                Username = username,
                Email = email,
                Password = hashedPassword,
                RoleId = playerRole.Id,
                Role = playerRole // On l'associe pour que le JwtProvider puisse lire son nom
            };

            // Sauvegarder en base de données
            await _userRepository.AddAsync(newUser);

            // Générer et retourner le token pour connecter l'utilisateur directement après son inscription
            return _jwtProvider.Generate(newUser);
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            // Chercher l'utilisateur
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("Identifiants incorrects.");
            }

            // Vérifier le mot de passe
            var isPasswordValid = _passwordHasher.Verify(password, user.Password);
            if (!isPasswordValid)
            {
                throw new Exception("Identifiants incorrects."); // On met le même message pour ne pas aider les hackers
            }

            // Générer le JWT
            return _jwtProvider.Generate(user);
        }
    }
}