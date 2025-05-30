using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingSystem.API.DTOs;
using ParkingSystem.API.DTOs.Requests;
using ParkingSystem.Application.Interfaces;
using ParkingSystem.Domain.Enums;
using ParkingSystem.Domain.Models;

namespace ParkingSystem.API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UsersController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("/createuser")]
    public async Task<IActionResult> CreateUser([FromBody] RegisterRequestDTO registerRequestDto, UserRole userRole)
    {
        var existingUser = await _userService.FindByUsernameAsync(registerRequestDto.Username);
        if (existingUser is not null) return Conflict("Username is already registered.");

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerRequestDto.Password);

        await _userService.RegisterUserAsync(username: registerRequestDto.Username, hashedPassword: hashedPassword, role: userRole);

        return Ok("User registered successfully.");
    }

    [HttpGet]
    [Route("/showalluser")]
    public async Task<IActionResult> ShowAllUsers()
    {
        List<User> users = await _userService.GetAllUsersAsync();

        if (users.Count == 0) return NotFound("Database empty");

        List<UserDto> userDtos = [];
        foreach (var user in users)
            userDtos.Add(_mapper.Map<UserDto>(user));
        
        return Ok(userDtos);
    }

    [HttpGet]
    [Route("/searchuser")]
    public async Task<IActionResult> SearchUser(string searchString)
    {
        List<User> users = await _userService.SearchUserAsync(searchString);

        if (users.Count == 0)
            return NotFound("No match found.");

        return Ok(users);
    }

    [HttpDelete]
    [Route("/deleteuser")]
    public async Task<IActionResult> DeleteUser(string username, string password)
    {
        User? user = await _userService.FindByUsernameAsync(username);
        if (user is null) return NotFound("Username doesn't exist.");

        bool isVerified = BCrypt.Net.BCrypt.Verify(password, user.HashedPassword);
        if (!isVerified) return BadRequest("Password wrong.");

        await _userService.RemoveUserAsync(username);

        return Ok("User deleted successfully.");
    }
}