using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingSystem.API.DTOs;
using ParkingSystem.API.DTOs.Requests;
using ParkingSystem.Application.Helpers;
using ParkingSystem.Application.Interfaces;
using ParkingSystem.Domain.Models;

namespace ParkingSystem.API.Controllers;

[Authorize]
[ApiController]
[Route("api/vehicles")]
public class VehiclesController : ControllerBase
{
    private IVehicleService _vehicleService;
    private IMapper _mapper;

    public VehiclesController(IVehicleService vehicleApiService, IMapper mapper)
    {
        _vehicleService = vehicleApiService;
        _mapper = mapper;
    }

    [HttpPost] [Authorize(Roles = "Admin")]
    [Route("/registervehicle")]
    public async Task<IActionResult> RegisterVehicle([FromBody] RegisterVehicleRequestDto request)
    {
        Result<Vehicle> result = await _vehicleService.RegisterVehicleAsync(request.Type, request.LicensePlate, request.Owner);
        if (result.Value is null) return BadRequest(result.Message);

        VehicleDto vehicleDto = _mapper.Map<VehicleDto>(result.Value);

        return CreatedAtAction(nameof(SearchByLicensePlate), new { licensePlate = vehicleDto.LicensePlate }, vehicleDto);
    }

    [HttpGet] [Authorize(Roles = "Admin,Guard")]
    [Route("/getallvehicle")]
    public async Task<IActionResult> GetAllVehicles()
    {
        List<Vehicle> vehicles = await _vehicleService.GetAllVehiclesAsync();

        if (vehicles.Count == 0) return NotFound("Database empty.");

        List<VehicleDto> vehicleDtos = [];
        foreach (var vehicle in vehicles)
            vehicleDtos.Add(_mapper.Map<VehicleDto>(vehicle));

        return Ok(vehicleDtos);
    }

    [HttpGet]
    [Route("/searchbyowner")]
    public async Task<IActionResult> SearchByOwner(string owner)
    {
        var vehicles = await _vehicleService.FindByOwnerAsync(owner);

        if (vehicles.Count == 0) return NotFound($"No matches for {owner}.");

        List<VehicleDto> vehicleDtos = [];
        foreach (var vehicle in vehicles)
            vehicleDtos.Add(_mapper.Map<VehicleDto>(vehicle));

        return Ok(vehicleDtos);
    }

    [HttpGet]
    [Route("/searchbylicense")]
    public async Task<IActionResult> SearchByLicensePlate(string licensePlate)
    {
        var vehicle = await _vehicleService.FindByLicensePlateAsync(licensePlate);

        if (vehicle is null) return NotFound("License plate not registered.");

        return Ok(_mapper.Map<VehicleDto>(vehicle));
    }

    [HttpPut] [Authorize(Roles = "Admin")]
    [Route("/editvehicleowner")]
    public async Task<IActionResult> EditVehicleOwner(string licensePlate, string newOwnerName)
    {
        var result = await _vehicleService.EditVehicleOwnerAsync(licensePlate, newOwnerName);

        if (result.Value is true) return Ok(result.Message);

        return NotFound(result.Message);
    }

    [HttpDelete] [Authorize(Roles = "Admin")]
    [Route("/unregistervehicle")]
    public async Task<IActionResult> UnregVehicle(string licensePlate)
    {
        var result = await _vehicleService.UnregVehicleAsync(licensePlate);

        if (result is false) return NotFound("Cannot delete non-existing vehicle.");

        else return Ok("Vehicle data permanently deleted.");
    }
}
