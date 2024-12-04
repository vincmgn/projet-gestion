using Microsoft.AspNetCore.Mvc;
using Back.Models;
using Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Back.Controllers
{
    /// <summary>
    /// API pour gérer les catégories.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Récupère toutes les catégories.
        /// </summary>
        /// <returns>Liste des catégories.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Category>), 200)]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        /// <summary>
        /// Récupère une catégorie par son ID.
        /// </summary>
        /// <param name="id">L'identifiant de la catégorie.</param>
        /// <returns>Une catégorie.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Category), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        /// <summary>
        /// Met à jour une catégorie existante.
        /// </summary>
        /// <param name="id">L'identifiant de la catégorie.</param>
        /// <param name="category">Les données de la catégorie.</param>
        /// <returns>Aucune donnée si réussi.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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
        /// Crée une nouvelle catégorie.
        /// </summary>
        /// <param name="category">Les données de la catégorie.</param>
        /// <returns>La catégorie créée.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Category), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        /// <summary>
        /// Supprime une catégorie existante.
        /// </summary>
        /// <param name="id">L'identifiant de la catégorie.</param>
        /// <returns>Aucune donnée si réussi.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
