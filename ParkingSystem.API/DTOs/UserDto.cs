using ParkingSystem.Domain.Enums;

namespace ParkingSystem.API.DTOs;

public class UserDto
{
    public string Username {get; set;}
    public UserRole Role {get; set;}
}