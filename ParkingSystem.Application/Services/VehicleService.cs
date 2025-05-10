using ParkingSystem.Application.Common.Interfaces;
using ParkingSystem.Domain.Enums;
using ParkingSystem.Domain.Models;
using ParkingSystem.Persistence.Repositories.Interfaces;

namespace ParkingSystem.Application.Services;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUserRepository _userRepository;

    public VehicleService(IVehicleRepository vehicleRepository, IUserRepository userRepository)
    {
        _vehicleRepository = vehicleRepository;
        _userRepository = userRepository;
    }

    public async Task<Vehicle?> RegisterVehicleAsync(VehicleType vehicleType, string licensePlate, string ownerUsername)
    {
        User? owner = await _userRepository.FindByUsernameAsync(ownerUsername);
        if (owner is null) return null;
        
        Vehicle vehicle = new Vehicle(licensePlate, vehicleType, owner);
        await _vehicleRepository.AddAsync(vehicle);
        return vehicle;
    }

    public async Task<List<Vehicle>> GetAllVehiclesAsync()
    {
        return await _vehicleRepository.GetAllAsync();
    }

    public async Task<Vehicle?> FindByLicensePlateAsync(string licensePlate)
    {
        return await _vehicleRepository.FindByLicensePlateAsync(licensePlate);
    }

    public async Task<List<Vehicle>> FindByOwnerAsync(string ownerName)
    {
        return await _vehicleRepository.FindAllAsync(v => v.Owner.Username == ownerName);
    }

    public async Task<bool> EditVehicleOwnerAsync(string licensePlate, string newOwnerName)
    {
        var vehicle = await _vehicleRepository.FindByLicensePlateAsync(licensePlate);
        var newOwner = await _userRepository.FindByUsernameAsync(newOwnerName);

        if (vehicle is null || newOwner is null) return false;
        
        await _vehicleRepository.UpdateVehicleOwnerAsync(vehicle, newOwner);
        return true;
    }

    public async Task<bool> UnregVehicleAsync(string licensePlate)
    {
        Vehicle? vehicle = await _vehicleRepository.FindByLicensePlateAsync(licensePlate);
        
        if (vehicle is not null)
        {
            await _vehicleRepository.RemoveAsync(vehicle);
            return true;
        }
        return false;
    }
}