namespace OrderManagement.Application.Interfaces;

using OrderManagement.Shared.DTOs;

public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(Guid userId, CreateOrderRequest request);
    Task<List<OrderDto>> GetOrdersByUserAsync(Guid userId);
    Task<OrderDto?> GetOrderByIdAsync(Guid userId, Guid orderId);
    Task<bool> CancelOrderAsync(Guid userId, Guid orderId);
}
