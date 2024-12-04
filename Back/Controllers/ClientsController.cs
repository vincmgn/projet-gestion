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
    /// API pour gérer les clients.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Récupère tous les clients
        /// </summary>
        /// <returns>Liste des clients</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _context.Clients.ToListAsync();
        }

        /// <summary>
        /// Récupère un client par son ID
        /// </summary>
        /// <param name="id">ID du client à récupérer</param>
        /// <returns>Le client correspondant à l'ID</returns>
        /// <response code="404">Client non trouvé</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        /// <summary>
        /// Met à jour les informations d'un client
        /// </summary>
        /// <param name="id">ID du client à mettre à jour</param>
        /// <param name="client">Le client avec les nouvelles données</param>
        /// <returns>Résultat de la mise à jour</returns>
        /// <response code="400">ID non valide ou données incorrectes</response>
        /// <response code="404">Client non trouvé</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }

            // Récupérer le client existant avec ses commandes
            var existingClient = await _context.Clients
                                                .Include(c => c.Orders) // Inclure les commandes
                                                .FirstOrDefaultAsync(c => c.Id == id);

            if (existingClient == null)
            {
                return NotFound();
            }

            // Mettre à jour les informations du client
            existingClient.Name = client.Name;
            existingClient.Address = client.Address;
            existingClient.Siret = client.Siret;

            // Si le client envoie de nouvelles commandes, on les remplace,
            // sinon on conserve les commandes existantes
            if (client.Orders != null && client.Orders.Any())
            {
                existingClient.Orders = client.Orders; // Remplacer la liste des commandes
            }
            else
            {
                // Si aucune commande n'est envoyée, on conserve les commandes existantes
                existingClient.Orders = existingClient.Orders ?? new List<Order>();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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
        /// Crée un nouveau client
        /// </summary>
        /// <param name="client">Le client à créer</param>
        /// <returns>Le client créé</returns>
        /// <response code="400">Données invalides</response>
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = client.Id }, client);
        }

        /// <summary>
        /// Supprime un client par son ID
        /// </summary>
        /// <param name="id">ID du client à supprimer</param>
        /// <returns>Résultat de la suppression</returns>
        /// <response code="404">Client non trouvé</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
