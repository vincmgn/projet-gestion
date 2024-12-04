using Back.Models;
using Backend.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Back.Controllers
{
    /// <summary>
    /// API pour gérer l'authentification.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Enregistre un nouvel utilisateur.
        /// </summary>
        /// <param name="user">Les informations de l'utilisateur à enregistrer</param>
        /// <returns>Utilisateur enregistré</returns>
        /// <response code="400">Le nom d'utilisateur existe déjà</response>
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(User user)
        {
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Username == user.Username);
            if (existingUser != null)
            {
                return BadRequest("Username already exists.");
            }

            var passwordHasher = new PasswordHasher<User>();
            user.Password = passwordHasher.HashPassword(user, user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        /// <summary>
        /// Connecte un utilisateur et génère un JWT.
        /// </summary>
        /// <param name="user">Les informations de connexion de l'utilisateur</param>
        /// <returns>Token JWT si l'utilisateur est authentifié avec succès</returns>
        /// <response code="400">Tentative de connexion invalide (mauvais identifiants)</response>
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(User user)
        {
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Username == user.Username);
            if (existingUser == null)
            {
                return BadRequest("Invalid login attempt.");
            }

            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(existingUser, existingUser.Password, user.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return BadRequest("Invalid login attempt.");
            }

            var token = GenerateJwtToken(existingUser);

            return Ok(new { token });
        }

        /// <summary>
        /// Déconnecte l'utilisateur (pas de gestion de session nécessaire avec JWT).
        /// </summary>
        /// <returns>Réponse OK si la déconnexion est réussie</returns>
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Pas de gestion de session, car c'est un JWT
            return Ok();
        }

        /// <summary>
        /// Génère un token JWT pour un utilisateur authentifié.
        /// </summary>
        /// <param name="user">Utilisateur pour lequel le token JWT sera généré</param>
        /// <returns>Le token JWT généré</returns>
        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
            };

            // Utilisation de la clé secrète depuis la configuration
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
