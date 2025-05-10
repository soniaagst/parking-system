using Microsoft.AspNetCore.Mvc;
using ParkingSystem.API.DTOs.Requests;
using ParkingSystem.API.Services.Auth;
using ParkingSystem.Application.Common.Interfaces;

namespace ParkingSystem.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;
    private IUserService _userService;

    public AuthController(TokenService tokenService, IUserService userService)
    {
        _tokenService = tokenService;
        _userService = userService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerDto)
    {
        var existingUser = await _userService.FindByUsernameAsync(registerDto.Username);
        if (existingUser is not null) return Conflict("Username is already registered.");

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        await _userService.RegisterUserAsync(username: registerDto.Username, hashedPassword: hashedPassword);

        return Ok("User registered successfully.");
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        var user = await _userService.FindByUsernameAsync(loginRequest.Username);
        if (user is null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.HashedPassword))
        {
            return Unauthorized("Invalid credentials.");
        }

        var token = _tokenService.GenerateJwtToken(user);
        return Ok(new { Token = token });
    }
}
