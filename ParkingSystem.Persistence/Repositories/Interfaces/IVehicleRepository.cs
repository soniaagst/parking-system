using ParkingSystem.Domain.Models;

namespace ParkingSystem.Persistence.Repositories.Interfaces;

public interface IVehicleRepository : IRepository<Vehicle>
{
    public Task<Vehicle?> FindByLicensePlateAsync(string licensePlate);
    public Task UpdateVehicleOwnerAsync(Vehicle vehicle, User newOwner);
}