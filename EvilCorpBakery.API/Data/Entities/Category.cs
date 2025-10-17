namespace EvilCorpBakery.API.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool isActive { get; set; } = true;
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
