# NetCore Aspire AppHost

This Aspire AppHost integrates with the root docker-compose infrastructure.

## Prerequisites

1. Docker and Docker Compose installed
2. .NET 10 SDK installed
3. Create the Docker network (one time only):
   ```bash
   docker network create netcore-network
   ```

## Running the Application

### Option 1: Using Aspire Dashboard (Recommended for Development)

1. **Start the infrastructure services** (from root directory):
   ```bash
   docker-compose -f docker-compose.local.yml up -d db redis
   ```

2. **Run the Aspire AppHost**:
   ```bash
   cd src/client/NetCore.AppHost
   dotnet run
   ```

   This will start:
   - API service on http://localhost:6000 and https://localhost:6001
   - UI service on http://localhost:6010 and https://localhost:6011
   - Migration service (runs once)
   - Aspire Dashboard on http://localhost:15888

### Option 2: Using Root Docker Compose (Full Container Mode)

Run everything as containers using the root docker-compose:
```bash
docker-compose -f docker-compose.local.yml up
```

This includes:
- SQL Server (db)
- Redis
- Migration service
- API service
- UI service (if uncommented)

## Configuration

### Environment Variables

The Aspire AppHost uses the following configuration:

- `SA_PASSWORD`: SQL Server password (default: "Your_strong_password123!")
- Database connection string points to `localhost:1433`

### Switching Between Modes

- **Aspire Mode**: Start only infrastructure (db, redis) with docker-compose, then run AppHost
- **Full Docker Mode**: Run entire stack with docker-compose
- **Hybrid Mode**: Run some services in containers, others via Aspire

## Benefits of Using Aspire

1. **Better Development Experience**: 
   - Live dashboard showing all services
   - Structured logs and metrics
   - Easy service-to-service communication testing

2. **Flexible**: 
   - Can run services locally for debugging
   - Infrastructure still uses proven docker-compose setup
   - No vendor lock-in

3. **Production Aligned**: 
   - Production uses docker-compose
   - Development can use either approach
   - Same infrastructure definitions

## Troubleshooting

### SQL Server Connection Issues

If you get connection errors, ensure:
1. SQL Server is running: `docker ps | grep db`
2. Port 1433 is available
3. Password matches in docker-compose and AppHost

### Port Conflicts

If ports are already in use, you can:
1. Stop conflicting services
2. Change ports in `Program.cs` (for Aspire mode)
3. Change ports in `docker-compose.local.yml` (for Docker mode)
