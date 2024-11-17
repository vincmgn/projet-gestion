﻿namespace Back.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime DatePeremption { get; set; }
        public int CategoryId { get; set; }
        public string Emplacement { get; set; }

        
        // Navigation property
        public Category Category { get; set; }  // Relation avec Category
    }
}
