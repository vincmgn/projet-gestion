using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Back.Models;

namespace Back.Controllers
{
    /// <summary>
    /// API pour gérer les commandes.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Récupère toutes les commandes
        /// </summary>
        /// <returns>Liste des commandes</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        /// <summary>
        /// Récupère une commande par son ID
        /// </summary>
        /// <param name="id">ID de la commande à récupérer</param>
        /// <returns>La commande correspondant à l'ID</returns>
        /// <response code="404">Commande non trouvée</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        /// <summary>
        /// Met à jour une commande
        /// </summary>
        /// <param name="id">ID de la commande à mettre à jour</param>
        /// <param name="order">Les nouvelles données de la commande</param>
        /// <returns>Résultat de la mise à jour</returns>
        /// <response code="400">ID non valide ou données incorrectes</response>
        /// <response code="404">Commande non trouvée</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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
        /// Crée une nouvelle commande
        /// </summary>
        /// <param name="order">La commande à créer</param>
        /// <returns>La commande créée</returns>
        /// <response code="400">Données invalides</response>
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        /// <summary>
        /// Supprime une commande par son ID
        /// </summary>
        /// <param name="id">ID de la commande à supprimer</param>
        /// <returns>Résultat de la suppression</returns>
        /// <response code="404">Commande non trouvée</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        /// <summary>
        /// Récupère le produit le plus vendu
        /// </summary>
        /// <returns>Le produit le plus vendu</returns>
        /// <response code="404">Aucune commande trouvée</response>
        /// <response code="200">Produit le plus vendu</response>
        [HttpGet("bestseller")]
        public async Task<ActionResult<object>> GetBestSeller()
        {
            var bestSeller = await _context.Orders
                .GroupBy(o => o.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    Quantity = g.Sum(o => o.Quantity)
                })
                .OrderByDescending(g => g.Quantity)
                .FirstOrDefaultAsync();

            if (bestSeller == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(bestSeller.ProductId);
            return new
            {
                ProductId = bestSeller.ProductId,
                ProductName = product.Name,
                Quantity = bestSeller.Quantity
            };
        }

        /// <summary>
        /// Récupère le Top 3 des produits les plus vendus
        /// </summary>
        /// <returns>Top 3 des produits les plus vendus</returns>
        /// <response code="404">Aucun produit trouvé dans le Top 3</response>
        /// <response code="200">Top 3 des produits les plus vendus</response>
        [HttpGet("top3")]
        public async Task<ActionResult<IEnumerable<object>>> GetTop3()
        {
            try
            {
                // Jointure pour récupérer les informations produit et calculer les quantités totales
                var top3 = await _context.Orders
                    .GroupBy(o => o.ProductId)
                    .Select(g => new
                    {
                        ProductId = g.Key,
                        Quantity = g.Sum(o => o.Quantity)
                    })
                    .OrderByDescending(g => g.Quantity)
                    .Take(3)
                    .Join(_context.Products,  // Jointure avec la table des produits
                          g => g.ProductId,
                          p => p.Id,
                          (g, p) => new
                          {
                              ProductId = g.ProductId,
                              ProductName = p.Name,
                              Quantity = g.Quantity
                          })
                    .ToListAsync();

                if (!top3.Any())
                {
                    return NotFound(new { Message = "Aucun produit trouvé dans le Top 3." });
                }

                return Ok(top3);
            }
            catch (Exception ex)
            {
                // Gestion des erreurs génériques
                return StatusCode(500, new { Message = "Erreur interne lors de la récupération du Top 3.", Details = ex.Message });
            }
        }



        /// <summary>
        /// Récupère le total des ventes
        /// </summary>
        /// <returns>Total des ventes</returns>
        /// <response code="200">Total des ventes</response>
        /// <response code="404">Aucune commande trouvée</response>
        [HttpGet("total")]
        public async Task<ActionResult<decimal>> GetTotalSales()
        {
            var totalSales = await _context.Orders
                .Select(o => (double)(o.Quantity * o.Product.Price)) // Convert to double
                .SumAsync();

            return (decimal)totalSales; // Convert back to decimal if needed
        }

    }
}
