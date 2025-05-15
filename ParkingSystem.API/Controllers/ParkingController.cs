using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingSystem.API.DTOs;
using ParkingSystem.Application.Interfaces;
using ParkingSystem.Domain.Models;

namespace ParkingSystem.API.Controllers;

[Authorize]
[ApiController]
[Route("api/parking")]
public class ParkingController : ControllerBase
{
    private IParkingService _parkingService;
    private IVehicleService _vehicleService;
    private IMapper _mapper;

    public ParkingController(IParkingService parkingService, IVehicleService vehicleService, IMapper mapper)
    {
        _parkingService = parkingService;
        _vehicleService = vehicleService;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("/gettingstarted")]
    public async Task<IActionResult> InitializeParkingLot(string name = "FmlxParkingArea", int carSlotCount = 60, int bikeSlotCount = 800)
    {
        bool result = await _parkingService.InitializeParkingLot(name, carSlotCount, bikeSlotCount);
        if (result)
            return Ok("Parking Lot created.");
        return BadRequest("Already initialized.");
    }

    [HttpPost]
    [Route("/park")]
    public async Task<IActionResult> ParkVehicle(string licensePlate)
    {
        Vehicle? vehicle = await _vehicleService.FindByLicensePlateAsync(licensePlate);

        if (vehicle is null) return NotFound("Vehicle not registered.");

        var ticketResponse = await _parkingService.IssueTicketAsync(vehicle);

        if (ticketResponse.Value is null) return NotFound(ticketResponse.Message);

        return Ok(_mapper.Map<TicketDto>(ticketResponse.Value));
    }

    [HttpPost]
    [Route("/unpark")]
    public async Task<IActionResult> UnparkVehicle(string licensePlate, string ticketId)
    {
        var feeResponse = await _parkingService.ProcessExitAsync(licensePlate, Guid.Parse(ticketId));

        if (feeResponse.Value == 0) return BadRequest(feeResponse.Message);

        return Ok(feeResponse.Message);
    }
}