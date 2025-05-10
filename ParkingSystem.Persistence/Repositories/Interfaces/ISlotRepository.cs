using ParkingSystem.Domain.Enums;
using ParkingSystem.Domain.Models;

namespace ParkingSystem.Persistence.Repositories.Interfaces;

public interface ISlotRepository : IRepository<Slot>
{
    Task<Slot?> FindAvailableSlotForAsync(VehicleType vehicleType);
}