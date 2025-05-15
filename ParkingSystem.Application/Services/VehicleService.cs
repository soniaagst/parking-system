using ParkingSystem.Application.Interfaces;
using ParkingSystem.Application.Helpers;
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

    public async Task<Result<Vehicle>> RegisterVehicleAsync(VehicleType vehicleType, string licensePlate, string ownerUsername)
    {
        Vehicle? existingVehicle = await _vehicleRepository.FindByLicensePlateAsync(licensePlate);
        if (existingVehicle is not null)
            return new Result<Vehicle>(null, $"Vehicle with license plate <{licensePlate}> already exist.");

        User? owner = await _userRepository.FindByUsernameAsync(ownerUsername);
        if (owner is null)
            return new Result<Vehicle>(null, $"No user with username <{ownerUsername}> found.");
        
        Vehicle vehicle = new(licensePlate, vehicleType, owner);
        await _vehicleRepository.AddAsync(vehicle);
        return new Result<Vehicle>(vehicle, "Vehicle registered successfully.");
    }

    public async Task<List<Vehicle>> GetAllVehiclesAsync()
    {
        return await _vehicleRepository.GetAllAsync(propertiesToInclude: "Owner");
    }

    public async Task<Vehicle?> FindByLicensePlateAsync(string licensePlate)
    {
        return await _vehicleRepository.FindByLicensePlateAsync(licensePlate);
    }

    public async Task<List<Vehicle>> FindByOwnerAsync(string ownerName)
    {
        return await _vehicleRepository.FindAllAsync(v => v.Owner.Username == ownerName, propertiesToInclude: "Owner");
    }

    public async Task<Result<bool>> EditVehicleOwnerAsync(string licensePlate, string newOwnerName)
    {
        var vehicle = await _vehicleRepository.FindByLicensePlateAsync(licensePlate);
        var newOwner = await _userRepository.FindByUsernameAsync(newOwnerName);

        if (vehicle is null) return new Result<bool>(false, "Vehicle not found.");
        if (newOwner is null) return new Result<bool>(false, $"User with username {newOwnerName} is not found.");
        
        await _vehicleRepository.UpdateVehicleOwnerAsync(vehicle, newOwner);
        return new Result<bool>(true, "Owner update success.");
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