namespace EvilCorpBakery.API.Models.DTO
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string OrderGuid { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
