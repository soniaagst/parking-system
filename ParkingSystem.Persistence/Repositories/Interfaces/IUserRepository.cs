using ParkingSystem.Domain.Models;

namespace ParkingSystem.Persistence.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    public Task<User?> FindByUsernameAsync(string username);
}