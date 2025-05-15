using ParkingSystem.Domain.Enums;
using ParkingSystem.Domain.Models;

namespace ParkingSystem.Application.Interfaces;

public interface IUserService
{
    Task RegisterUserAsync(string username, string hashedPassword, UserRole role);
    Task<List<User>> GetAllUsersAsync();
    Task<User?> FindByUsernameAsync(string username);
    Task<List<User>> SearchUserAsync(string searchString);
    Task<bool> RemoveUserAsync(string username);
}