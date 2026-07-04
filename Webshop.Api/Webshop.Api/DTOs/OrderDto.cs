using Webshop.Api.Models;

namespace Webshop.Api.DTOs
{
    public class OrderDto
    {
        public int Id {  get; set; }
        public DateTime CreatedAt {  get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
