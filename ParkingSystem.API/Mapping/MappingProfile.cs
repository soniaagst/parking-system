using AutoMapper;
using ParkingSystem.API.DTOs;
using ParkingSystem.Domain.Models;

namespace ParkingSystem.API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Vehicle, VehicleDto>()
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Username));

        CreateMap<Ticket, TicketDto>();

        CreateMap<User, UserDto>();
    }
}