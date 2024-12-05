using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Back.Models;
using Backend.Data;

namespace Back.Controllers
{
    /// <summary>
    /// API pour gérer les produits.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Récupère tous les produits
        /// </summary>
        /// <returns>Liste des produits</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        /// <summary>
        /// Récupère un produit par son ID
        /// </summary>
        /// <param name="id">ID du produit à récupérer</param>
        /// <returns>Le produit correspondant à l'ID</returns>
        /// <response code="404">Produit non trouvé</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        /// <summary>
        /// Met à jour un produit existant
        /// </summary>
        /// <param name="id">ID du produit à mettre à jour</param>
        /// <param name="product">Le produit à mettre à jour</param>
        /// <returns>Résultat de la mise à jour</returns>
        /// <response code="400">ID non valide ou données incorrectes</response>
        /// <response code="404">Produit non trouvé</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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
        /// Crée un nouveau produit
        /// </summary>
        /// <param name="product">Le produit à créer</param>
        /// <returns>Le produit créé</returns>
        /// <response code="400">Données invalides, catégorie non trouvée</response>
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            // Vérifie si l'ID de la catégorie est valide
            if (product.CategoryId == null)
            {
                return BadRequest("L'ID de la catégorie est obligatoire.");
            }

            var category = await _context.Categories.FindAsync(product.CategoryId);
            if (category == null)
            {
                return BadRequest("La catégorie spécifiée n'existe pas.");
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        /// <summary>
        /// Supprime un produit par son ID
        /// </summary>
        /// <param name="id">ID du produit à supprimer</param>
        /// <returns>Résultat de la suppression</returns>
        /// <response code="404">Produit non trouvé</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        /// <summary>
        /// Récupère les produits dont la quantité est inférieure à 5 (stock faible)
        /// </summary>
        /// <returns>Liste des produits en stock faible</returns>
        [HttpGet("lowstock")]
        public async Task<ActionResult<IEnumerable<string>>> GetLowStockProducts()
        {
            // Requête pour récupérer les produits dont la quantité est inférieure à 5
            var lowStockProducts = await _context.Products
                .Where(p => p.Quantity < 5)  // Filtrer les produits avec un stock inférieur à 5
                .Select(p => $"{p.Name} - Stock < 5")  // Vous pouvez choisir d'afficher plus d'informations si nécessaire
                .ToListAsync();

            // Si aucun produit en stock faible n'est trouvé, on retourne un message
            if (!lowStockProducts.Any())
            {
                return NoContent();
            }

            return Ok(lowStockProducts);
        }
    }
}
