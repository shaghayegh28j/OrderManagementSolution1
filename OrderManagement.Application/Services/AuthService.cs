using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Shared.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrderManagement.Application.Services;

public class AuthService
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher<User> _hasher;
    private readonly IConfiguration _config;

    public AuthService(
        IUserRepository users,
        IPasswordHasher<User> hasher,
        IConfiguration config)
    {
        _users = users;
        _hasher = hasher;
        _config = config;
    }

    public async Task RegisterAsync(RegisterRequest req)
    {
        if (await _users.GetByEmailAsync(req.Email) != null)
            throw new Exception("User exists");

        var user = new User { Email = req.Email };
        user.PasswordHash = _hasher.HashPassword(user, req.Password);

        await _users.AddAsync(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest req)
    {
        var user = await _users.GetByEmailAsync(req.Email)
            ?? throw new Exception("Invalid credentials");

        var ok = _hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);
        if (ok == PasswordVerificationResult.Failed)
            throw new Exception("Invalid credentials");

        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]!);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(6),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256));

        return new AuthResponse(
            new JwtSecurityTokenHandler().WriteToken(token),
            user.Email,
            user.Role);
    }
}
