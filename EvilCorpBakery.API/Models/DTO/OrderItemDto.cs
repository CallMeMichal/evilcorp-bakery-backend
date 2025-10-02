namespace EvilCorpBakery.API.Models.DTO
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public ProductDTO ProductDTO { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal => Quantity * UnitPrice;
    }
}
