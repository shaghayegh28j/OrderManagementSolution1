namespace OrderManagement.Domain.Entities
{
    public enum OrderStatus { Pending, Processing, Shipped, Delivered, Cancelled }


    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
