using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;
using OrderManagementSolution.OrdeManagement.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace OrderManagement.Application.UseCases
{
    public class LoginUserUseCase
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _config;


        public LoginUserUseCase(IUserRepository userRepo, IPasswordHasher<User> passwordHasher, IConfiguration config)
        {
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
            _config = config;
        }


        public async Task<AuthResponse> ExecuteAsync(LoginRequest req)
        {
            var user = await _userRepo.GetByEmailAsync(req.Email) ?? throw new InvalidOperationException("Invalid credentials");
            var ver = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);
            if (ver == PasswordVerificationResult.Failed) throw new InvalidOperationException("Invalid credentials");


            // create JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"] ?? throw new InvalidOperationException("Missing JWT Key"));
            var claims = new List<Claim>
{
new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
new Claim(ClaimTypes.Email, user.Email),
new Claim(ClaimTypes.Role, user.Role)
};


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new AuthResponse(tokenHandler.WriteToken(token), user.Email, user.Role);
        }
    }
}