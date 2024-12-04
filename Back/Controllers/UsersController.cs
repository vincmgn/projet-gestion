using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Back.Models;
using System.Security.Claims;

namespace Back.Controllers
{
    /// <summary>
    /// API pour gérer les utilisateurs.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Récupère tous les utilisateurs
        /// </summary>
        /// <returns>Liste des utilisateurs</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Récupère un utilisateur par son ID
        /// </summary>
        /// <param name="id">ID de l'utilisateur à récupérer</param>
        /// <returns>L'utilisateur correspondant à l'ID</returns>
        /// <response code="404">Utilisateur non trouvé</response>
        /// <response code="401">Non autorisé à accéder aux données d'un autre utilisateur</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Vérifie que l'utilisateur essaie d'accéder à ses propres données
            if (id != userId)
            {
                return Unauthorized();  // Non autorisé à accéder aux données d'un autre utilisateur
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        /// <summary>
        /// Met à jour un utilisateur
        /// </summary>
        /// <param name="id">ID de l'utilisateur à mettre à jour</param>
        /// <param name="user">Les nouvelles données de l'utilisateur</param>
        /// <returns>Résultat de la mise à jour</returns>
        /// <response code="400">ID non valide ou données incorrectes</response>
        /// <response code="404">Utilisateur non trouvé</response>
        /// <response code="401">Non autorisé à modifier un autre utilisateur</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Vérifie que l'utilisateur ne modifie pas les données d'un autre utilisateur
            if (id != userId)
            {
                return Unauthorized();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Crée un nouvel utilisateur
        /// </summary>
        /// <param name="user">Les données de l'utilisateur à créer</param>
        /// <returns>L'utilisateur créé</returns>
        /// <response code="400">Données invalides</response>
        /// <response code="401">Non autorisé à créer un utilisateur pour un autre</response>
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            // Vérifie si l'utilisateur a un rôle d'admin avant de créer un nouvel utilisateur
            var roleClaim = User.FindFirst(ClaimTypes.Role);
            if (roleClaim == null || roleClaim.Value != "Admin")
            {
                return Unauthorized("Only admins can create users.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        /// <summary>
        /// Supprime un utilisateur par son ID
        /// </summary>
        /// <param name="id">ID de l'utilisateur à supprimer</param>
        /// <returns>Résultat de la suppression</returns>
        /// <response code="404">Utilisateur non trouvé</response>
        /// <response code="401">Non autorisé à supprimer un autre utilisateur</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Vérifie si l'utilisateur a un rôle d'admin avant de supprimer un utilisateur
            var roleClaim = User.FindFirst(ClaimTypes.Role);
            if (roleClaim == null || roleClaim.Value != "Admin")
            {
                return Unauthorized("Only admins can delete users.");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
