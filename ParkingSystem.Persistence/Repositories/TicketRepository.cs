using ParkingSystem.Domain.Models;
using ParkingSystem.Persistence.Data;
using ParkingSystem.Persistence.Repositories.Interfaces;

namespace ParkingSystem.Persistence.Repositories;

public class TicketRepository : Repository<Ticket>, ITicketRepository
{
    private ParkingDbContext _dbContext;

    public TicketRepository(ParkingDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Ticket>> GetActiveTicketsAsync()
    {
        return await FindAllAsync(t => t.IsActive);
    }

    public async Task<Ticket?> FindActiveByLicenseAsync(string licensePlate)
    {
        var activeTickets = await GetActiveTicketsAsync();
        return activeTickets.Where(t => t.Vehicle.LicensePlate == licensePlate).FirstOrDefault();
    }
}