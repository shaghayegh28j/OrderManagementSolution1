using OrderManagement.Application.DTOS;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.UseCases
{
    public class CreateOrderUseCase
    {
        private readonly IOrderRepository _orderRepo;

        public CreateOrderUseCase(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<OrderDto> ExecuteAsync(Guid userId, CreateOrderRequest req)
        {
            var order = new Order
            {
                UserId = userId,
                ProductName = req.ProductName,
                Quantity = req.Quantity
            };

            await _orderRepo.AddAsync(order);

            return new OrderDto(
                order.Id,
                order.ProductName,
                order.Quantity,
                order.Status,
                order.CreatedAt
            );
        }
    }
}
