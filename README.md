# NetCore

[![.NET](https://github.com/NghiaNguyen170192/NetCore/actions/workflows/netcore-ci.yml/badge.svg)](https://github.com/NghiaNguyen170192/NetCore/actions/workflows/netcore-ci.yml)

## About
An ongoing project using .NET Core 10.
Purpose of this project is to learn and implement Clean Architecture:
- Domain Driven Design
- Clean Architecture
- .NET Aspire
- Docker
- CI/CD

<br />

## Local Development Setup (Recommended)

For local development, we use **.NET Aspire AppHost** which provides a better development experience with live dashboard, telemetry, and easier debugging.

### Prerequisites
- .NET 10 SDK
- Docker Desktop (for infrastructure services only)

### Running Locally with Aspire

1. **Start infrastructure services** (PostgreSQL and Redis):
   ```bash
   docker network create netcore-network
   docker compose -f docker-compose.local.yml up -d db redis
   ```

2. **Run the Aspire AppHost**:
   ```bash
   cd src/client/NetCore.AppHost
   dotnet run
   ```

3. **Access the applications**:
   - **Aspire Dashboard**: http://localhost:15888 (logs, metrics, traces)
   - **API**: http://localhost:6000 or https://localhost:6001
   - **UI**: http://localhost:6010 or https://localhost:6011
   - **API Swagger**: https://localhost:6001/swagger/index.html
   - **Redis Insight**: http://localhost:8001

The Aspire AppHost will automatically:
- Run database migrations
- Start the API service
- Start the UI service
- Provide live telemetry and logs

<br />

## Production Deployment

For production, use the full Docker Compose setup:

### Build and Deploy

1. **Generate HTTPS certificates** (first time only):
   ```bash
   dotnet dev-certs https -ep .\certificates\.netcore-api\https\netcore-api.pfx -p aJ3oPVRd6vPWndrqSf4gYFsc5P3BYM --trust
   ```

2. **Build Docker images**:
   ```bash
   docker compose --env-file .\.env -f .\docker-compose.prod.yml build
   ```

3. **Create Docker network** (first time only):
   ```bash
   docker network create netcore-network
   ```

4. **Start all services**:
   ```bash
   docker compose --env-file .\.env -f .\docker-compose.prod.yml up -d
   ```

5. **Access the application**:
   - **API**: http://localhost:6000 or https://localhost:6001
   - **Dozzle (Log Viewer)**: http://localhost:8080
   - **Redis**: localhost:6379

<br />

## Database Migrations

**Add application database migration**
```bash
cd .\src\NetCore.Infrastructure.Database\

dotnet ef migrations add migration_name --context ApplicationDatabaseContext -o .\Migrations\
```

**Add Identity Server Store migration**
```bash
cd .\src\NetCore.Infrastructure.AuthenticationDatabase\

dotnet ef migrations add migration_name --context ApplicationDbContext -o .\Migrations\ApplicationDb
```

<br />

## Architecture Overview

- **NetCore.Domain**: Domain entities, events, and interfaces
- **NetCore.Application**: Application services, CQRS commands/queries
- **NetCore.Infrastructure.Database**: EF Core, repositories, database context
- **NetCore.Api**: REST API endpoints
- **NetCore.UI**: Blazor WebAssembly frontend
- **NetCore.AppHost**: .NET Aspire orchestration for local development
- **NetCore.Migration**: Database seeding and migration tool

<br />

## Key Features

- ? Clean Architecture with DDD
- ? CQRS with domain events
- ? PostgreSQL database
- ? Redis distributed caching
- ? .NET Aspire for local development
- ? Docker containerization for production
- ? Entity Framework Core migrations
- ? Swagger/OpenAPI documentation
- ? Health checks
- ? Structured logging

<br />

## Development vs Production

| Aspect | Local Development | Production |
|--------|------------------|------------|
| Orchestration | .NET Aspire AppHost | Docker Compose |
| Services | Run as .NET processes | Run as Docker containers |
| Infrastructure | Docker (PostgreSQL, Redis only) | Docker (all services) |
| Debugging | Full .NET debugging | Container logs via Dozzle |
| Dashboard | Aspire Dashboard (port 15888) | Dozzle (port 8080) |
| Hot Reload | Supported | Not applicable |
| Telemetry | Built-in with Aspire | Custom logging |

<br />

## Troubleshooting

### Clean up infrastructure
```bash
docker compose -f .\docker-compose.local.yml down -v
```

### Reset PostgreSQL data
```bash
docker compose -f .\docker-compose.local.yml down -v
docker volume rm netcore_postgres_data
```

### View logs
- **Aspire mode**: Check Aspire Dashboard at http://localhost:15888
- **Docker mode**: Use Dozzle at http://localhost:8080 or `docker logs <container_name>`