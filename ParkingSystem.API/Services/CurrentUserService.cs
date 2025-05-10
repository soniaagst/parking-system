using ParkingSystem.Application.Common.Interfaces;
using System.Security.Claims;

namespace ParkingSystem.API.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId => 
        Guid.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value, out var id)
            ? id : null;

    public string? Username => 
        _httpContextAccessor.HttpContext?.User?.Identity?.Name;

    public string? Role => 
        _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
}
