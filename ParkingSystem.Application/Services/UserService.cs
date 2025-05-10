using ParkingSystem.Application.Common.Interfaces;
using ParkingSystem.Domain.Enums;
using ParkingSystem.Domain.Models;
using ParkingSystem.Persistence.Repositories.Interfaces;

namespace ParkingSystem.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task RegisterUserAsync(string username, string hashedPassword)
    {
        User user = new User (username, hashedPassword, UserRole.Member);

        await _userRepository.AddAsync(user);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<User?> FindByUsernameAsync(string username)
    {
        var user = await _userRepository.FindByUsernameAsync(username);

        if (user is null) return null;

        return user;
    }

    public async Task<bool> RemoveUserAsync(string username, string hashedPassword)
    {
        var user = await FindByUsernameAsync(username);

        if (user is not null && user.HashedPassword == hashedPassword)
        {
            await _userRepository.RemoveAsync(user);
            return true;
        }
        return false;
    }
}