using ParkingSystem.Domain.Models;

namespace ParkingSystem.Application.Services;

public class ParkingLotCache
{
    private ParkingLotConfig _cachedConfig;
    public ParkingLotConfig GetConfig() => _cachedConfig;
    public void SetConfig(ParkingLotConfig config) => _cachedConfig = config;
}