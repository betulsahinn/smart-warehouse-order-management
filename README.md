# Smart Warehouse and Order Management System

Production-oriented .NET 8 Web API using Clean Architecture:

- `Swoms.Domain`: entities, value objects, enums, domain rules
- `Swoms.Application`: feature services, DTOs, validation, mapping, repository contracts
- `Swoms.Infrastructure`: EF Core SQL Server persistence, repositories, unit of work, JWT and refresh-token services
- `Swoms.API`: controllers, authentication, Swagger, Serilog, exception middleware

## Run locally

```powershell
dotnet restore
dotnet build
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=SwomsDb;User Id=sa;Password=<password>;TrustServerCertificate=True;MultipleActiveResultSets=True" --project src/Swoms.API
dotnet user-secrets set "Jwt:Secret" "<at-least-32-character-random-secret>" --project src/Swoms.API
dotnet run --project src/Swoms.API/Swoms.API.csproj
```

Swagger is available at `/swagger` in development.

## Docker

```powershell
copy .env.example .env
docker compose up --build
```

The API listens on `http://localhost:8080` and SQL Server listens on `localhost,1433`.

## Database

Create migrations from the repository root after packages are restored:

```powershell
dotnet ef migrations add InitialCreate --project src/Swoms.Infrastructure --startup-project src/Swoms.API --output-dir Persistence/Migrations
dotnet ef database update --project src/Swoms.Infrastructure --startup-project src/Swoms.API
```

Use environment variables or a secret store for production connection strings and JWT secrets.
