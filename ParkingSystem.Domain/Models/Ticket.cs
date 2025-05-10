using System.ComponentModel.DataAnnotations;
using ParkingSystem.Domain.Enums;

namespace ParkingSystem.Domain.Models;

public class Ticket
{
    [Key]
    public Guid Id { get; }
    public Vehicle Vehicle { get; set; }
    public DateTime EnterTime { get; }
    public DateTime? ExitTime { get; private set; }
    public bool IsActive { get; private set; }

    private Ticket() { }

    public Ticket(Vehicle vehicle)
    {
        Id = Guid.NewGuid();
        Vehicle = vehicle;
        EnterTime = DateTime.Now;
        IsActive = true;
    }

    public void EndParking()
    {
        ExitTime = DateTime.Now.AddHours(Random.Shared.Next(1, 13));
        IsActive = false;
    }

    public TimeSpan GetDuration()
    {
        return (ExitTime ?? DateTime.Now) - EnterTime;
    }

    public double CalculateFee()
    {
        double duration = GetDuration().TotalHours;
        return double.Round(duration * HourlyRate[Vehicle.Type], 2);
    }

    readonly Dictionary<VehicleType, int> HourlyRate = new() {
        {VehicleType.Car, 2_000},
        {VehicleType.Motorcycle, 1_000}
    };
}