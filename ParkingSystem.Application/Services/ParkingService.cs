using ParkingSystem.Application.Common.Interfaces;
using ParkingSystem.Application.Helpers;
using ParkingSystem.Domain.Enums;
using ParkingSystem.Domain.Models;
using ParkingSystem.Persistence.Data;
using ParkingSystem.Persistence.Repositories.Interfaces;

namespace ParkingSystem.Application.Services;

public class ParkingService : IParkingService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly ISlotRepository _slotRepository;
    private readonly ParkingDbContext _dbContext;

    public ParkingService(IVehicleRepository vehicleRepository, ITicketRepository ticketRepository, ISlotRepository slotRepository, ParkingDbContext dbContext)
    {
        _vehicleRepository = vehicleRepository;
        _ticketRepository = ticketRepository;
        _slotRepository = slotRepository;
        _dbContext = dbContext;
    }

    public async Task<Result<Ticket>> IssueTicketAsync(Vehicle vehicle)
    {
        Ticket? ticket = null;
        string message;

        Ticket? existingTicket = await _ticketRepository.FindActiveByLicenseAsync(vehicle.LicensePlate);
        if (existingTicket is not null)
        {
            message = "Vehicle already has an active ticket.";
            return new Result<Ticket>(ticket, message);
        }

        Slot? freeSpace = await _slotRepository.FindAvailableSlotForAsync(vehicle.Type);
        if (freeSpace is null)
        {
            message = $"No free space for {vehicle.Type}.";
            return new Result<Ticket>(ticket, message);
        }

        ticket = new(vehicle);
        await _ticketRepository.AddAsync(ticket);

        freeSpace.Park(vehicle);
        await _slotRepository.UpdateAsync(freeSpace);

        message = $"Vehicle {vehicle.LicensePlate} parked at slot #{freeSpace.SlotNumber}. You got a parking ticket!";
        return new Result<Ticket>(ticket, message);
    }

    public async Task<Result<double>> ProcessExitAsync(string licensePlate, Guid ticketId)
    {
        string message;
        double fee = 0;

        Ticket? ticket = await _ticketRepository.FindByIdAsync(ticketId);

        if (ticket is null)
        {
            message = "Ticket not found.";
            return new Result<double>(fee, message);
        }

        if (!ticket.IsActive)
        {
            message = "Ticket is not active.";
            return new Result<double>(fee, message);
        }

        Vehicle? vehicle = await _vehicleRepository.FindFirstAsync(v => v.LicensePlate == licensePlate);

        if (vehicle is null)
        {
            message = "Vehicle not found.";
            return new Result<double>(fee, message);
        }

        bool validated = vehicle.LicensePlate == ticket.Vehicle.LicensePlate;
        if (!validated)
        {
            message = "Invalid. Ticket doesn't belong to this vehicle.";
            return new Result<double>(fee, message);
        }

        Slot? occupiedSlot = await _slotRepository.FindFirstAsync(slot => slot.ParkedVehicle == vehicle);
        if (occupiedSlot == null) {
            message = "Vehicle not parked.";
            return new Result<double>(fee, message);
        }

        occupiedSlot.Unpark();
        await _slotRepository.UpdateAsync(occupiedSlot);
        ticket.EndParking();
        await _ticketRepository.UpdateAsync(ticket);
        fee = ticket.CalculateFee();
        message = $"Vehicle {vehicle.LicensePlate} exited | Parking fee: Rp{fee} | Enter time: {ticket.EnterTime} | Exit time: {ticket.ExitTime}";
        return new Result<double>(fee, message);
    }

    public async Task<bool> InitializeParkingLot(string name, int carSlotCount, int bikeSlotCount)
    {
        if (_dbContext.ParkingLotConfigs.Any()) return false;

        ParkingLotConfig config = new()
        {
            Name = name,
            CarSlotCount = carSlotCount,
            BikeSlotCount = bikeSlotCount
        };
        await _dbContext.ParkingLotConfigs.AddAsync(config);
        await _dbContext.SaveChangesAsync();

        List<Slot> Slots = [];
        int slotNumber = 1;

        for (int i = 0; i < config.CarSlotCount; i++)
        {
            Slots.Add(new Slot(slotNumber, VehicleType.Car));
            slotNumber++;
        }
        for (int i = 0; i < config.BikeSlotCount; i++)
        {
            Slots.Add(new Slot(slotNumber, VehicleType.Motorcycle));
            slotNumber++;
        }

        await _slotRepository.AddRangeAsync(Slots);

        return true;
    }
}