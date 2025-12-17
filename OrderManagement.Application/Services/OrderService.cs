namespace OrderManagement.Application.Services;

using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
using OrderManagement.Shared.DTOs;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repo;

    public OrderService(IOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task<OrderDto> CreateOrderAsync(Guid userId, CreateOrderRequest request)
    {
        var order = new Order(userId, request.ProductName, request.Quantity);
        await _repo.AddAsync(order);
        return OrderDto.From(order);
    }

    public async Task<List<OrderDto>> GetOrdersByUserAsync(Guid userId)
    {
        var orders = await _repo.GetByUserIdAsync(userId);
        return orders.Select(OrderDto.From).ToList();
    }

    public async Task<OrderDto?> GetOrderByIdAsync(Guid userId, Guid orderId)
    {
        var order = await _repo.GetByIdAsync(orderId);
        if (order == null || order.UserId != userId)
            return null;

        return OrderDto.From(order);
    }

    public async Task<bool> CancelOrderAsync(Guid userId, Guid orderId)
    {
        var order = await _repo.GetByIdAsync(orderId);
        if (order == null || order.UserId != userId)
            return false;

        if (order.Status is OrderStatus.Shipped or OrderStatus.Delivered)
            return false;

        order.Cancel();
        await _repo.UpdateAsync(order);
        return true;
    }
}
