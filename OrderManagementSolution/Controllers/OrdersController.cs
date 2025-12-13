using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs;         
using OrderManagement.Application.DTOS;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.UseCases;
using OrderManagement.Domain.Entities;           
using System.Security.Claims;
namespace OrderManagement.API.Controllers         
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly CreateOrderUseCase _createOrder;
        private readonly IOrderRepository _orderRepo;

        public OrdersController(CreateOrderUseCase createOrder, IOrderRepository orderRepo)
        {
            _createOrder = createOrder;
            _orderRepo = orderRepo;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create(CreateOrderRequest req)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var orderDto = await _createOrder.ExecuteAsync(userId, req);
            return Ok(orderDto);
        }

        [HttpGet("me")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetMyOrders()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var orders = await _orderRepo.GetByUserIdAsync(userId);

            var orderDtos = orders.Select(o => new OrderDto(
                o.Id,
                o.ProductName,
                o.Quantity,
                o.Status,
                o.CreatedAt
            ));

            return Ok(orderDtos);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll()
        {
            var orders = await _orderRepo.GetAllAsync();

            var orderDtos = orders.Select(o => new OrderDto(
                o.Id,
                o.ProductName,
                o.Quantity,
                o.Status,
                o.CreatedAt
            ));

            return Ok(orderDtos);
        }

        [HttpPut("{id:guid}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] string status)
        {
            var order = await _orderRepo.GetByIdAsync(id);

            if (order == null)
                return NotFound();

            if (!Enum.TryParse<OrderStatus>(status, true, out var parsedStatus))
                return BadRequest("Invalid status value");

            order.Status = parsedStatus;
            await _orderRepo.UpdateAsync(order);

            return NoContent();
        }
    }
}