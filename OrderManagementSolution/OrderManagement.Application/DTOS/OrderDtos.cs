using System;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.DTOS
{
   
    public record CreateOrderRequest(string ProductName, int Quantity);

    public record OrderDto(
        Guid Id,
        string ProductName,
        int Quantity,
        OrderStatus Status, 
        DateTime CreatedAt
    );
}