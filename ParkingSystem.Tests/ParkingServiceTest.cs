using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Internal;
using ParkingSystem.Application.Services;
using ParkingSystem.Domain.Enums;
using ParkingSystem.Domain.Models;
using ParkingSystem.Persistence.Data;
using ParkingSystem.Persistence.Repositories;

namespace ParkingSystem.Tests;

[TestFixture]
public class ParkingServiceTests
{
    private ParkingService _parkingService;
    private VehicleRepository _vehicleRepository;
    private TicketRepository _ticketRepository;
    private SlotRepository _slotRepository;
    private ParkingDbContext _dbContext;

    [SetUp]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<ParkingDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _dbContext = new(options);
        _dbContext.Database.EnsureCreated();

        _vehicleRepository  = new(_dbContext);
        _ticketRepository = new(_dbContext);
        _slotRepository = new(_dbContext);

        _parkingService = new(_vehicleRepository, _ticketRepository, _slotRepository, _dbContext);

        await _parkingService.InitializeParkingLot("TestParkingSlot", 1, 1);
    }

    [TearDown]
    public void Teardown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    private static readonly List<User> users = 
    [  
        new User("Anton", "", UserRole.Admin),
        new User("Bagus", "", UserRole.Member),
        new User("Candra", "", UserRole.Guest),
        new User("Dadang", "", UserRole.Member),
        new User("Fitri", "", UserRole.Member)
    ];

    private static IEnumerable<Vehicle> VehicleTestCases()
    {
        yield return new Vehicle{Type = VehicleType.Motorcycle, LicensePlate = "A1234B", Owner = users[0]};
        yield return new Vehicle{Type = VehicleType.Motorcycle, LicensePlate = "B1234C", Owner = users[1]};
        yield return new Vehicle{Type = VehicleType.Car, LicensePlate = "C1234D", Owner = users[2]};
        yield return new Vehicle{Type = VehicleType.Car, LicensePlate = "D1234E", Owner = users[3]};
    }

    [Test, TestCaseSource(nameof(VehicleTestCases))]
    public async Task Park_Should_ReturnTicket(Vehicle vehicle)
    {
        var ticket = await _parkingService.IssueTicketAsync(vehicle);
        Assert.That(ticket.Value, Is.InstanceOf<Ticket>());
    }

    [Test]
    public async Task Park_Should_Not_ReturnTicket_When_Full()
    {
        List<Vehicle> vehicles = VehicleTestCases().ToList();

        User stranger = new User("guest", "", UserRole.Guest);
        Vehicle otherVehicle = new Vehicle{Type = VehicleType.Car, LicensePlate = "T3355T", Owner = stranger};

        List<Ticket?> tickets = [];

        foreach (var vhc in vehicles)
        {
            await _vehicleRepository.AddAsync(vhc);
            var t = await _parkingService.IssueTicketAsync(vhc);
            tickets.Add(t.Value);
        }

        var ticket = await _parkingService.IssueTicketAsync(otherVehicle);

        Assert.That(tickets[0], Is.Not.Null);
        Assert.That(tickets[0], Is.InstanceOf<Ticket>());
        Assert.That(tickets[1], Is.Null);
        Assert.That(ticket.Value, Is.Null);
    }

    [Test]
    public async Task Unpark_Should_ReturnDouble()
    {
        Vehicle vehicle = new Vehicle{Type = VehicleType.Motorcycle, LicensePlate = "F1234G", Owner = users[4]};

        await _vehicleRepository.AddAsync(vehicle);

        var ticket = await _parkingService.IssueTicketAsync(vehicle);
        
        var result = await _parkingService.ProcessExitAsync("F1234G", ticket.Value!.Id);

        Assert.That(result.Value, Is.InstanceOf<double>());
    }
}