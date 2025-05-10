using FluentValidation;
using ParkingSystem.API.DTOs.Requests;

namespace ParkingSystem.API.Validators;

public class RegisterVehicleValidator : AbstractValidator<RegisterVehicleRequestDto>
{
    public RegisterVehicleValidator()
    {
        RuleFor(v => v.LicensePlate)
            .NotEmpty().WithMessage("License plate is required.")
            .Matches(@"^[A-Z]{1,2}[0-9]{4}[A-Z]{1,2}$")
            .WithMessage("License plate format: XX1234XX.");

        RuleFor(v => v.Owner)
            .NotEmpty().WithMessage("Owner name is required.")
            .MinimumLength(3).WithMessage("Owner name must be at least 3 characters long.");

        RuleFor(v => v.Type)
            .IsInEnum().WithMessage("Invalid vehicle type.");
    }
}
