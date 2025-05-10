using System.ComponentModel.DataAnnotations;
using ParkingSystem.Domain.Enums;

namespace ParkingSystem.Domain.Models;

public class Slot
{
    [Key]
    public int SlotNumber { get; }
    public VehicleType AllowedType { get; }
    public Vehicle? ParkedVehicle { get; private set; }
    public bool IsOccupied { get; private set; }

    private Slot() { }

    public Slot(int slotNumber, VehicleType allowedType)
    {
        SlotNumber = slotNumber;
        AllowedType = allowedType;
    }

    public bool Park(Vehicle vehicle)
    {
        if (IsOccupied) return false;
        ParkedVehicle = vehicle;
        IsOccupied = true;
        return true;
    }

    public bool Unpark()
    {
        if (!IsOccupied) return false;
        ParkedVehicle = null;
        IsOccupied = false;
        return true;
    }
}