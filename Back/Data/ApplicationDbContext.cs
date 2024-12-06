using Microsoft.EntityFrameworkCore;
using Back.Models;
using Microsoft.AspNetCore.Identity;

namespace Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .HasMaxLength(50)
                .IsRequired();

            // Product
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Client
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Client)
                .HasForeignKey(o => o.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany()
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed des données pour la table Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Furniture" },
                new Category { Id = 3, Name = "Clothing" }
            );

            // Seed des données pour la table Products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Quantity = 4, Price = 1000m, DatePeremption = DateTime.Now.AddYears(2), CategoryId = 1, Emplacement = "A1" },
                new Product { Id = 2, Name = "Chair", Quantity = 25, Price = 150m, DatePeremption = DateTime.Now.AddYears(5), CategoryId = 2, Emplacement = "B2" },
                new Product { Id = 3, Name = "T-shirt", Quantity = 50, Price = 20m, DatePeremption = DateTime.Now.AddYears(1), CategoryId = 3, Emplacement = "C3" }
            );

            // Seed des données pour la table Clients
            modelBuilder.Entity<Client>().HasData(
                new Client { Id = 1, Name = "Client1", Address = "123 Main St", Siret = "1234567890" },
                new Client { Id = 2, Name = "Client2", Address = "456 Market Rd", Siret = "0987654321" }
            );

            // Seed des données pour la table Orders
            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, ClientId = 1, ProductId = 1, Quantity = 2, DateCommande = DateTime.Now, Statut = "En attente" },
                new Order { Id = 2, ClientId = 2, ProductId = 3, Quantity = 5, DateCommande = DateTime.Now, Statut = "Livrée" },
                new Order { Id = 3, ClientId = 1, ProductId = 2, Quantity = 1, DateCommande = DateTime.Now, Statut = "Expédiée" }
            );

            // Seed des données pour la table Users
            var passwordHasher = new PasswordHasher<User>();
            var hashedPassword = passwordHasher.HashPassword(null, "admin");
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", Password = hashedPassword }
            );
        }
    }
}
