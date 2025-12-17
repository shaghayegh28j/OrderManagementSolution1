namespace OrderManagement.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Interfaces;
using OrderManagement.Shared.DTOs;
using System.Security.Claims;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _service;

    public OrdersController(IOrderService service)
    {
        _service = service;
    }

    private Guid GetUserId()
    {
        var claim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
            User.FindFirst("sub")?.Value;

        if (!Guid.TryParse(claim, out var userId))
            throw new UnauthorizedAccessException("Invalid user token");

        return userId;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderRequest request)
    {
        var userId = GetUserId();
        return Ok(await _service.CreateOrderAsync(userId, request));
    }

    [HttpGet]
    public async Task<IActionResult> GetMyOrders()
    {
        var userId = GetUserId();
        return Ok(await _service.GetOrdersByUserAsync(userId));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var userId = GetUserId();
        var order = await _service.GetOrderByIdAsync(userId, id);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelOrder(Guid id)
    {
        var userId = GetUserId();
        var success = await _service.CancelOrderAsync(userId, id);
        return success ? NoContent() : NotFound();
    }
}
