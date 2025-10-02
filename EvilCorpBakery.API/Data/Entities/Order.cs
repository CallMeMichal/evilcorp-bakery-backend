namespace EvilCorpBakery.API.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }

        // Foreign Key do User
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        // Foreign Key do Address
        public int AddressId { get; set; }
        public virtual Address Address { get; set; } = null!;

        // Kolekcja pozycji zamówienia
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        // Status zamówienia
        public string Status { get; set; } = "Pending";

        // Łączna kwota
        public decimal TotalAmount { get; set; }

        // Notatki do zamówienia
        public string? Notes { get; set; }

        // Daty
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}