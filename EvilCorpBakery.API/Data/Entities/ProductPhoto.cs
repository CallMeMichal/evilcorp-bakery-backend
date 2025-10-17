namespace EvilCorpBakery.API.Data.Entities
{
    public class ProductPhoto
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsMain { get; set; } = false;
        public int ProductId { get; set; }


        public virtual Product Product { get; set; } = null!;
    }
}