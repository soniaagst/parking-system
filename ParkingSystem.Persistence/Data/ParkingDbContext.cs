using Microsoft.EntityFrameworkCore;
using ParkingSystem.Domain.Models;

namespace ParkingSystem.Persistence.Data;

public class ParkingDbContext : DbContext
{
    public ParkingDbContext(DbContextOptions<ParkingDbContext> options) : base(options) { }

    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Slot> Slots { get; set; }
    public DbSet<ParkingLotConfig> ParkingLotConfigs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Vehicle>(vehicle =>
        {
            vehicle.HasKey(v => v.LicensePlate);
            vehicle.Property(v => v.Type);
        });

        modelBuilder.Entity<User>(user =>
        {
            user.HasKey(u => u.Id);
            user.HasIndex(u => u.Username);
        });

        modelBuilder.Entity<Ticket>(ticket =>
        {
            ticket.HasKey(t => t.Id);
            ticket.Property(t => t.EnterTime);
            ticket.Property(t => t.ExitTime);
            ticket.Property(t => t.IsActive);
        });

        modelBuilder.Entity<Slot>(slot =>
        {
            slot.HasKey(s => s.SlotNumber);
            slot.Property(s => s.AllowedType);
        });
    }
}