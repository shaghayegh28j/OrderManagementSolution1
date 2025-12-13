namespace OrderManagement.Application.DTOs
{
    public record AuthResponse(string Token, string Email, string Role);
}
