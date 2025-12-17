using OrderManagement.Domain.Enums;
namespace OrderManagement.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Order() { }

    public Order(Guid userId, string productName, int quantity)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        ProductName = productName;
        Quantity = quantity;
        Status = OrderStatus.Created;
        CreatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        Status = OrderStatus.Cancelled;
    }
}
