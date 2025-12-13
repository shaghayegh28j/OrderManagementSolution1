using Microsoft.AspNetCore.Identity;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;
using OrderManagementSolution.OrdeManagement.Domain.Entities;

namespace OrderManagement.Application.UseCases
{
    public class RegisterUserUseCase
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher<User> _passwordHasher;

        public RegisterUserUseCase(IUserRepository userRepo, IPasswordHasher<User> passwordHasher)
        {
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
        }

        public async Task ExecuteAsync(RegisterRequest req)
        {
            if (req == null)
                throw new ArgumentNullException(nameof(req));

            var exist = await _userRepo.GetByEmailAsync(req.Email);
            if (exist != null)
                throw new InvalidOperationException("Email already registered");

            var user = new User
            {
                Email = req.Email,
                PasswordHash = "", 
                Role = "User",
                CreatedAt = DateTime.UtcNow 
            };

            // hash password
            user.PasswordHash = _passwordHasher.HashPassword(user, req.Password);

            await _userRepo.AddAsync(user);
        }
    }
}