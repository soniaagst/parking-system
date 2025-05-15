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
            User kangParkir = new("jukir", "$2a$11$O2UmKIwW0.blqguheuQ/LOwR94ri3jtq7LIjsRRynPd3NUQBcoRJ.", UserRole.Guard);
            User member = new("naruto", "$2a$11$iraOS.gEFeFsKwQ4kw9Wpe9UTpiQPRT85hi0HFpwKifKX1Y1WMwJm", UserRole.Member);
            Vehicle car = new("A 4444 A", VehicleType.Car, adminUser);
            Vehicle bike = new("K 1412 K", VehicleType.Motorcycle, member);

            dbContext.Users.AddRange(adminUser, kangParkir, member);
            dbContext.Vehicles.AddRange(car, bike);
            dbContext.SaveChanges();
        }
    }
}