using Microsoft.EntityFrameworkCore;
using ParkingSystem.Domain.Models;
using ParkingSystem.Persistence.Data;
using ParkingSystem.Persistence.Repositories.Interfaces;

namespace ParkingSystem.Persistence.Repositories;

public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
{
    private ParkingDbContext _dbContext;

    public VehicleRepository(ParkingDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Vehicle?> FindByLicensePlateAsync(string licensePlate)
    {
        return await _dbContext.Vehicles.FirstOrDefaultAsync(vhcl => vhcl.LicensePlate == licensePlate);
    }

    public async Task UpdateVehicleOwnerAsync(Vehicle vehicle, User newOwner)
    {
        vehicle.Owner = newOwner;
        await _dbContext.SaveChangesAsync();
    }
}