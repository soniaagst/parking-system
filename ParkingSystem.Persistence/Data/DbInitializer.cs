using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ParkingSystem.Domain.Enums;
using ParkingSystem.Domain.Models;

namespace ParkingSystem.Persistence.Data;

public static class DbInitializer
{
    public static void Seed(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ParkingDbContext>();

        dbContext.Database.Migrate();

        if (!dbContext.Users.Any())
        {
            User adminUser = new("admin", "$2a$11$UVu01etRZTQnGGGdxQFIRexZfDcpS5ExvI7AIJgr65G6ECGseQBsC", UserRole.Admin);
            User member = new("naruto", "$2a$11$iraOS.gEFeFsKwQ4kw9Wpe9UTpiQPRT85hi0HFpwKifKX1Y1WMwJm", UserRole.Member);
            Vehicle car = new("A4444A", VehicleType.Car, member);
            Vehicle bike = new("K1412K", VehicleType.Motorcycle, adminUser);

            dbContext.Users.AddRange(adminUser, member);
            dbContext.Vehicles.AddRange(car, bike);
            dbContext.SaveChanges();
        }
    }
}