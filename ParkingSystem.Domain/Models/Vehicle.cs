using System.ComponentModel.DataAnnotations;
using ParkingSystem.Domain.Enums;

namespace ParkingSystem.Domain.Models;

public class Vehicle
{
    [Key]
    public string LicensePlate { get; set; }
    public VehicleType Type { get; set; }
    public User Owner { get; set; }

    public Vehicle() { }

    public Vehicle(string licensePlate, VehicleType type, User owner)
    {
        LicensePlate = licensePlate;
        Type = type;
        Owner = owner;
    }

    public override string ToString()
    {
        return $"{Type} - {LicensePlate} - {Owner.Username}";
    }
}