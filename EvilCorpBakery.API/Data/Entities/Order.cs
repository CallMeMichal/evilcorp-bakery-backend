namespace EvilCorpBakery.API.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderGuid { get; set; } = string.Empty;
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public int? AddressId { get; set; }
        public virtual Address Address { get; set; } = null!;

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public int StatusId { get; set; }
        public virtual OrderStatus Status { get; set; } = null!;

        public decimal TotalAmount { get; set; }

        public string? Notes { get; set; }
        public int PaymentMethod { get; set; }
        public virtual PaymentTypes PaymentTypes { get; set; } = null!;
        // Daty
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}