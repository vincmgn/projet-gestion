using System.Text.Json.Serialization;

namespace Back.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation property
        [JsonIgnore]
        public List<Product>? Products { get; set; } = new List<Product>();
    }
}
