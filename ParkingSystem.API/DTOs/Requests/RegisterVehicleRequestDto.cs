using ParkingSystem.Domain.Enums;

namespace ParkingSystem.API.DTOs.Requests;

public class RegisterVehicleRequestDto
{
    public string LicensePlate { get; set; }
    public VehicleType Type { get; set; }
    public string Owner { get; set; }
}
