using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Infrastructure.Persistence;

namespace OrderManagement.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _db;

    public OrderRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Order order)
    {
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
    }

    public async Task<List<Order>> GetByUserIdAsync(Guid userId)
    {
        return await _db.Orders
            .Where(o => o.UserId == userId)
            .ToListAsync();
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _db.Orders.ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _db.Orders.FindAsync(id);
    }

    public async Task UpdateAsync(Order order)
    {
        _db.Orders.Update(order);
        await _db.SaveChangesAsync();
    }
}
