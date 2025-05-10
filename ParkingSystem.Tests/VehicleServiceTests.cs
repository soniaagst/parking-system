using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Internal;
using ParkingSystem.Application.Services;
using ParkingSystem.Domain.Enums;
using ParkingSystem.Domain.Models;
using ParkingSystem.Persistence.Data;
using ParkingSystem.Persistence.Repositories;

namespace ParkingSystem.Tests;

[TestFixture]
public class VehicleServiceTests
{
    private VehicleService _vehicleService;
    private VehicleRepository _vehicleRepository;
    private UserRepository _userRepository;
    private ParkingDbContext _dbContext;

    [SetUp]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<ParkingDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _dbContext = new(options);
        _dbContext.Database.EnsureCreated();

        _vehicleRepository = new(_dbContext);
        _userRepository = new(_dbContext);

        _vehicleService = new(_vehicleRepository, _userRepository);

        await _userRepository.AddRangeAsync([
            new User("Dadang", "", UserRole.Member),
            new User("Endah", "", UserRole.Member)]);
    }

    [TearDown]
    public void Teardown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task RegisterVehicle_Should_AddVehicleToDatabase()
    {
        await _vehicleService.RegisterVehicleAsync(VehicleType.Car, "D1234E", "Dadang");

        List<Vehicle> result = await _vehicleRepository.GetAllAsync();

        Assert.That(result, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task UnregVehicle_InputLicense_ReturnTrue()
    {
        await _vehicleService.RegisterVehicleAsync(VehicleType.Car, "E1234F", "Endah");

        var result = await _vehicleService.UnregVehicleAsync("E1234F");

        Assert.That(result, Is.True);
    }
}