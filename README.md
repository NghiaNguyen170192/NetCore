# NetCore.Donation

[![.NET](https://github.com/NghiaNguyen170192/NetCore.Donation/actions/workflows/netcore-ci.yml/badge.svg)](https://github.com/NghiaNguyen170192/NetCore.Donation/actions/workflows/netcore-ci.yml)

## About
NetCore.Donation is a .NET 10 donation-management API based on the NetCore Clean Architecture template.

The current local-first scaffold includes:
- Country and contact management
- Payment methods and recurring payment schedules
- Donation transactions and receipts
- PostgreSQL persistence with EF Core migrations
- MediatR CQRS, OData queries, Problem Details errors, health checks, and OpenTelemetry
- .NET Aspire orchestration for PostgreSQL, Redis, migration/seeding, API, and the template UI

Architecture:
- Domain Driven Design
- Clean Architecture
- .NET Aspire
- Docker
- MediatR CQRS

<br />

## Local Development Setup (Recommended)

The Aspire AppHost creates the required local containers, applies EF Core migrations, and inserts idempotent sample data.

### Prerequisites
- .NET 10 SDK
- Docker Desktop

### Running Locally with Aspire

1. **Run the Aspire AppHost from the repository root**:
   ```bash
   dotnet run --project src/client/NetCore.Donation.AppHost
   ```

2. **Access the applications**:
   - **Aspire Dashboard**: use the URL printed by AppHost
   - **API**: http://localhost:6000 or https://localhost:6001
   - **UI**: http://localhost:6010 or https://localhost:6011
   - **API Swagger**: https://localhost:6001/swagger/index.html

The Aspire AppHost will automatically:
- Start PostgreSQL and Redis
- Apply database migrations
- Seed countries and a sample donation workflow
- Start the API service
- Start the UI service
- Provide live telemetry and logs

### API routes

- `/api/v1/countries`
- `/api/v1/contacts`
- `/api/v1/payment-methods`
- `/api/v1/payment-schedules`
- `/api/v1/transactions`
- `/api/v1/receipts`
- `/health`

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
cd .\src\NetCore.Donation.Infrastructure.Database\

dotnet ef migrations add migration_name --context ApplicationDatabaseContext -o .\Migrations\
```

**Add Identity Server Store migration**
```bash
cd .\src\NetCore.Donation.Infrastructure.AuthenticationDatabase\

dotnet ef migrations add migration_name --context ApplicationDbContext -o .\Migrations\ApplicationDb
```

<br />

## Architecture Overview

- **NetCore.Donation.Domain**: Domain entities, events, and interfaces
- **NetCore.Donation.Application**: Application services, CQRS commands/queries
- **NetCore.Donation.Infrastructure.Database**: EF Core, repositories, database context
- **NetCore.Donation.Api**: REST API endpoints
- **NetCore.Donation.UI**: Blazor WebAssembly frontend
- **NetCore.Donation.AppHost**: .NET Aspire orchestration for local development
- **NetCore.Donation.Migration**: Database seeding and migration tool

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