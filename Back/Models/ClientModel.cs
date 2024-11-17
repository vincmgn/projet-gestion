namespace Back.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Siret { get; set; }

        // Navigation property (pour accéder aux commandes de ce client)
        public List<Order> Orders { get; set; }  // Relation un-à-plusieurs avec Order
    }
}
