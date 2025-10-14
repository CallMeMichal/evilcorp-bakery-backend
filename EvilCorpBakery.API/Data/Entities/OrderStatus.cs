namespace EvilCorpBakery.API.Data.Entities
{
    public class OrderStatus
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
