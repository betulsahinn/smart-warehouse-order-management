FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Swoms.sln", "./"]
COPY ["src/Swoms.Domain/Swoms.Domain.csproj", "src/Swoms.Domain/"]
COPY ["src/Swoms.Application/Swoms.Application.csproj", "src/Swoms.Application/"]
COPY ["src/Swoms.Infrastructure/Swoms.Infrastructure.csproj", "src/Swoms.Infrastructure/"]
COPY ["src/Swoms.API/Swoms.API.csproj", "src/Swoms.API/"]
RUN dotnet restore "Swoms.sln"
COPY . .
RUN dotnet publish "src/Swoms.API/Swoms.API.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Swoms.API.dll"]
