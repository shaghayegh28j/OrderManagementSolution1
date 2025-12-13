using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.UseCases;
using OrderManagement.Application.DTOs;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly RegisterUserUseCase _register;
    private readonly LoginUserUseCase _login;


    public AuthController(RegisterUserUseCase register, LoginUserUseCase login)
    {
        _register = register;
        _login = login;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest req)
    {
        try
        {
            await _register.ExecuteAsync(req);
            return Ok(new { message = "Registered" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req)
    {
        try
        {
            var r = await _login.ExecuteAsync(req);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}