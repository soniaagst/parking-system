using System.ComponentModel.DataAnnotations;

namespace ParkingSystem.Domain.Models;

public sealed class ParkingLotConfig
{
    [Key]
    public string Name { get; set; }
    public int CarSlotCount { get; set; }
    public int BikeSlotCount { get; set; }
}