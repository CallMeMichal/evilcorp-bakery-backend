namespace EvilCorpBakery.API.Models.Domain
{
    public class OrderItemDomain
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? Base64Image { get; set; }
    }
}
