using Microsoft.EntityFrameworkCore;
using ParkingSystem.Domain.Enums;
using ParkingSystem.Domain.Models;
using ParkingSystem.Persistence.Data;
using ParkingSystem.Persistence.Repositories.Interfaces;

namespace ParkingSystem.Persistence.Repositories;

public class SlotRepository : Repository<Slot>, ISlotRepository
{
    private readonly ParkingDbContext _dbContext;
    
    public SlotRepository(ParkingDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Slot?> FindAvailableSlotForAsync(VehicleType vehicleType)
    {
        return await _dbContext.Slots.FirstOrDefaultAsync(s => !s.IsOccupied && (s.AllowedType == vehicleType));
    }
}