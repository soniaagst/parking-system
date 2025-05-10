This application is for internal parking system of an instance, company, org, etc.

---
Navigate to ParkingSystem.API and run. It automatically creates .db at the first run.

---
    Users Seed
Username | Password
-------- | --------
admin    | admin
naruto   | dattebayo

---
How to add new migrations from ParkingSystem.API:
```
dotnet ef migrations add MigrationsName --project ..\ParkingSystem.Persistence\ --startup-project .
```