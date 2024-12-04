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
    }
}
