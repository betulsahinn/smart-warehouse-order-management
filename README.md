# Smart Warehouse and Order Management System

Production-oriented .NET 8 Web API using Clean Architecture:

- `Swoms.Domain`: entities, value objects, enums, domain rules
- `Swoms.Application`: feature services, DTOs, validation, mapping, repository contracts
- `Swoms.Infrastructure`: EF Core PostgreSQL persistence, repositories, unit of work, JWT and refresh-token services
- `Swoms.API`: controllers, authentication, Swagger, Serilog, exception middleware

## Run locally

```powershell
dotnet restore
dotnet build
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=swoms;Username=swoms_app;Password=<password>" --project src/Swoms.API
dotnet user-secrets set "Jwt:Secret" "<at-least-32-character-random-secret>" --project src/Swoms.API
dotnet run --project src/Swoms.API/Swoms.API.csproj
```

Swagger is available at `/swagger` in development.

## Docker

```powershell
copy .env.example .env
docker compose up --build
```

The API listens on `http://localhost:8080` and PostgreSQL listens on `localhost:5432`.
Set `POSTGRES_DB`, `POSTGRES_USER`, and `POSTGRES_PASSWORD` in `.env` before starting Docker Compose.

## Database

An initial PostgreSQL migration is included. Apply it from the repository root after packages are restored:

```powershell
dotnet ef database update --project src/Swoms.Infrastructure --startup-project src/Swoms.API
```

Create future migrations with:

```powershell
dotnet ef migrations add <MigrationName> --project src/Swoms.Infrastructure --startup-project src/Swoms.API --output-dir Persistence/Migrations
```

Use environment variables or a secret store for production connection strings and JWT secrets.
