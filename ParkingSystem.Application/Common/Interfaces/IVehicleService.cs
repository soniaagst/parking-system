using ParkingSystem.Domain.Enums;
using ParkingSystem.Domain.Models;

namespace ParkingSystem.Application.Common.Interfaces;

public interface IVehicleService
{
    Task<Vehicle?> RegisterVehicleAsync(VehicleType vehicleType, string licensePlate, string owner);
    Task<List<Vehicle>> GetAllVehiclesAsync();
    Task<Vehicle?> FindByLicensePlateAsync(string licensePlate);
    Task<List<Vehicle>> FindByOwnerAsync(string ownerName);
    Task<bool> EditVehicleOwnerAsync(string licensePlate, string newOwnerName);
    Task<bool> UnregVehicleAsync(string licensePlate);
}