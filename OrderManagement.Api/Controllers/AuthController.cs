using System;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Shared.DTOs
{
    public record OrderDto(
        Guid Id,
        string ProductName,
        int Quantity,
        string Status,
        DateTime CreatedAt
    )
    {
        public static OrderDto From(Order order)
        {
            return new OrderDto(
                order.Id,
                order.ProductName,
                order.Quantity,
                order.Status.ToString(),
                order.CreatedAt
            );
        }
    }
}