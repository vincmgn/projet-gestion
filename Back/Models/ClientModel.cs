using System.Text.Json.Serialization;

namespace Back.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Siret { get; set; }

        // Navigation property
        [JsonIgnore]
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
