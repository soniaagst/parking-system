using ParkingSystem.Application.Helpers;
using ParkingSystem.Domain.Models;

namespace ParkingSystem.Application.Common.Interfaces;

public interface IParkingService
{
    Task<Result<Ticket>> IssueTicketAsync(Vehicle vehicle);
    Task<Result<double>> ProcessExitAsync(string licensePlate, Guid ticketId);
    Task<bool> InitializeParkingLot(string name, int carSlotCount, int bikeSlotCount);
}