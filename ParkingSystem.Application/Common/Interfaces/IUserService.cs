using ParkingSystem.Domain.Models;

namespace ParkingSystem.Application.Common.Interfaces;

public interface IUserService
{
    Task RegisterUserAsync(string username, string hashedPassword);
    Task<List<User>> GetAllUsersAsync();
    Task<User?> FindByUsernameAsync(string username);
    Task<bool> RemoveUserAsync(string username, string hashedPassword);
}