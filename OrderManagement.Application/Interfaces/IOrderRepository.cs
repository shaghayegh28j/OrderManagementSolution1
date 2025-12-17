using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task<List<Order>> GetByUserIdAsync(Guid userId);
    Task<List<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(Guid id);
    Task UpdateAsync(Order order);
}
