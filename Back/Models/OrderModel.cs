namespace Back.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime DateCommande { get; set; }
        public string Statut { get; set; }


        // Navigation properties
        public Client Client { get; set; }  // Relation avec Client
        public Product Product { get; set; }  // Relation avec Product
    }
}
