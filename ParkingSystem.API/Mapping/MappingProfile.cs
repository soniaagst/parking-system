using AutoMapper;
using ParkingSystem.API.DTOs;
using ParkingSystem.Domain.Models;

namespace ParkingSystem.API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Vehicle, VehicleDto>();

        CreateMap<Ticket, TicketDto>();

        CreateMap<User, UserDto>();
    }
}