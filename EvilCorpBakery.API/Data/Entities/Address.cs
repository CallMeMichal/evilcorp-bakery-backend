namespace EvilCorpBakery.API.Data.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        // Dane adresowe
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        // Opcjonalnie - czy jest to domyślny adres
        public bool IsDefault { get; set; } = false;

        // Opcjonalnie - etykieta (np. "Dom", "Praca")
        public string? Label { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
