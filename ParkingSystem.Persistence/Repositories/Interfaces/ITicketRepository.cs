using ParkingSystem.Domain.Models;

namespace ParkingSystem.Persistence.Repositories.Interfaces;

public interface ITicketRepository : IRepository<Ticket>
{
    Task<List<Ticket>> GetActiveTicketsAsync();
    Task<Ticket?> FindActiveByLicenseAsync(string licensePlate);
}