using System.ComponentModel.DataAnnotations;
using ParkingSystem.Domain.Enums;

namespace ParkingSystem.Domain.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Username { get; set; }
    public string HashedPassword { get; set; }
    public UserRole Role { get; set; }

    private User() { }

    public User(string username, string hashedPassword, UserRole role)
    {
        Id = Guid.NewGuid();
        Username = username;
        HashedPassword = hashedPassword;
        Role = role;
    }
}