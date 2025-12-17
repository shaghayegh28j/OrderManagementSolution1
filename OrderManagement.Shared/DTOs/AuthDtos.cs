
namespace OrderManagement.Shared.DTOs
{
    public record RegisterRequest(string Email, string Password);
    public record LoginRequest(string Email, string Password);
    public record AuthResponse(string Token, string Email, string Role);
    public record CreateOrderRequest(string ProductName, int Quantity);
}