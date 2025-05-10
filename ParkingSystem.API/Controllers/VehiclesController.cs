using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingSystem.API.DTOs;
using ParkingSystem.API.DTOs.Requests;
using ParkingSystem.Application.Common.Interfaces;
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

    [HttpPost]
    [Route("/registervehicle")]
    public async Task<IActionResult> RegisterVehicle([FromBody] RegisterVehicleRequestDto request)
    {
        var vehicle = await _vehicleService.RegisterVehicleAsync(request.Type, request.LicensePlate, request.Owner);

        VehicleDto vehicleDto = _mapper.Map<VehicleDto>(vehicle);

        return CreatedAtAction(nameof(SearchByLicensePlate), new { licensePlate = vehicleDto.LicensePlate }, vehicleDto);
    }

    [HttpGet]
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

    [HttpPut]
    [Route("/editvehicleowner")]
    public async Task<IActionResult> EditVehicleOwner(string licensePlate, string newOwnerName)
    {
        var result = await _vehicleService.EditVehicleOwnerAsync(licensePlate, newOwnerName);

        if (result is true) return Ok("Owner name updated.");

        return NotFound("Can't edit non-existing data.");
    }

    [HttpDelete]
    [Route("/unregistervehicle")]
    public async Task<IActionResult> UnregVehicle(string licensePlate)
    {
        var result = await _vehicleService.UnregVehicleAsync(licensePlate);

        if (result is false) return NotFound("Can't delete non-existing data.");

        else return Ok("Vehicle data permanently deleted.");
    }
}
