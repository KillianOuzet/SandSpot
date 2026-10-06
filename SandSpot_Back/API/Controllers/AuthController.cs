using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using API.DTOs;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        // Le contrôleur demande le IAuthService, que le Program.cs lui injectera
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var token = await _authService.RegisterAsync(request.Username, request.Email, request.Password);
                
                // On renvoie un objet JSON contenant le token avec un code 200 (OK)
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                // Si le service lève une erreur (ex: "Cet email est déjà utilisé"), on renvoie un code 400
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _authService.LoginAsync(request.Email, request.Password);
                
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}