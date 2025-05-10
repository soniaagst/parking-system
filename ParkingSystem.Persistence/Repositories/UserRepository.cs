using Microsoft.EntityFrameworkCore;
using ParkingSystem.Domain.Models;
using ParkingSystem.Persistence.Data;
using ParkingSystem.Persistence.Repositories.Interfaces;

namespace ParkingSystem.Persistence.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    private ParkingDbContext _dbContext;

    public UserRepository(ParkingDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> FindByUsernameAsync(string username)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
    }
}