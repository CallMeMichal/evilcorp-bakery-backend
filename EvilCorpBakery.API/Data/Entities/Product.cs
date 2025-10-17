﻿namespace EvilCorpBakery.API.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool isActive { get; set; } = true;

        public virtual ICollection<ProductPhoto> Photos { get; set; } = new List<ProductPhoto>();
        public virtual Category Category { get; set; } = null!;
    }
}
