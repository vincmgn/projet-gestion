using System.Text.Json.Serialization;

namespace Back.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime DatePeremption { get; set; }
        public int? CategoryId { get; set; }
        public string Emplacement { get; set; }

        // Navigation property
        [JsonIgnore]
        public Category? Category { get; set; }
    }
}
