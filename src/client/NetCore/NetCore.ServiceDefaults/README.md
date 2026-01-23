# Centralized Configuration Guide

## Overview

All shared application configurations are centralized in the `NetCore.ServiceDefaults` project. This ensures consistency across all microservices while allowing service-specific overrides.

## Architecture

### Configuration Hierarchy (Priority from highest to lowest):

1. **Service-specific appsettings.{Environment}.json** (e.g., `NetCore.Api/appsettings.Development.json`)
2. **Service-specific appsettings.json** (e.g., `NetCore.Api/appsettings.json`)
3. **Shared appsettings.{Environment}.json** (`NetCore.ServiceDefaults/appsettings.Development.json`)
4. **Shared appsettings.json** (`NetCore.ServiceDefaults/appsettings.json`)

## Centralized Configuration Sections

All services automatically inherit these configurations from `NetCore.ServiceDefaults`:

### 1. Database Configuration
```json
{
  "Database": {
    "ApplicationConnectionString": "...",
    "IdpConnectionString": "...",
    "Provider": "mssql",
    "MigrationsAssembly": "NetCore.Infrastructure.Database",
    "RedisConnectionString": "..."
  }
}
```

### 2. Authentication Server
```json
{
  "AuthenticationServer": {
    "Audience": "netcore-api",
    "Issuer": "https://localhost:6003/",
    "SecretKey": "..."
  }
}
```

### 3. Logging
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Information"
    },
    "Path": "D:\\Logs"
  }
}
```

### 4. OpenTelemetry
```json
{
  "OpenTelemetry": {
    "ServiceName": "NetCore",
    "ServiceVersion": "1.0.0"
  }
}
```

## Usage in Services

### 1. Enable Shared Configuration

In your service's `Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);

// This automatically loads shared configuration from ServiceDefaults
builder.AddServiceDefaults();

// Rest of your configuration...
```

### 2. Access Configuration via Dependency Injection

```csharp
using Microsoft.Extensions.Options;
using NetCore.ServiceDefaults.Configuration;

public class MyService
{
    private readonly DatabaseOptions _dbOptions;
    private readonly AuthenticationServerOptions _authOptions;
    
    public MyService(
        IOptions<DatabaseOptions> dbOptions,
        IOptions<AuthenticationServerOptions> authOptions)
    {
        _dbOptions = dbOptions.Value;
        _authOptions = authOptions.Value;
    }
}
```

### 3. Service-Specific Overrides

Each service should only contain service-specific settings in their `appsettings.json`:

**NetCore.Api/appsettings.json:**
```json
{
  "Api": {
    "EnableSwagger": true,
    "CorsOrigins": ["https://localhost:7001"]
  }
}
```

**NetCore.Migration/appsettings.json:**
```json
{
}
```

> **Note:** NetCore.Migration uses **command-line arguments** (`-d -m -s -t`) instead of configuration settings.  
> See `NetCore.Migration/README.md` for command-line interface documentation.

**NetCore.UI/appsettings.json:**
```json
{
  "UI": {
    "ApiBaseUrl": "https://localhost:6001"
  }
}
```

## Environment-Specific Configuration

### Development
- Located in `NetCore.ServiceDefaults/appsettings.Development.json`
- Contains actual connection strings and debug settings
- Automatically loaded when `ASPNETCORE_ENVIRONMENT=Development`

### Production
- Located in `NetCore.ServiceDefaults/appsettings.Production.json`
- Uses tokenized values (e.g., `#{Database.ApplicationConnectionString}`)
- Tokens should be replaced by your deployment pipeline

## Docker Support

### Configuration Loading Strategy

1. **Development Mode**: Loads from file system using relative paths
2. **Docker/Production**: 
   - First attempts to load from `/app/shared-config` volume mount
   - Falls back to embedded resources if volume not available

### Docker Compose Example

```yaml
services:
  netcore-api:
    image: netcoreapi
    volumes:
      # Mount shared configuration (optional)
      - ./src/client/NetCore/NetCore.ServiceDefaults:/app/shared-config:ro
    environment:
      - ASPNETCORE_ENVIRONMENT=Development

  netcore-migration:
    image: netcoremigration
    command: ["-d", "-m", "-s"]  # Command-line arguments
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
    depends_on:
      - db
```

## Available Configuration Classes

All configuration classes are registered as singletons and available via `IOptions<T>`:

- `DatabaseOptions` - Database connection settings
- `AuthenticationServerOptions` - Authentication/authorization settings  
- `LoggingOptions` - Logging configuration
- `OpenTelemetryOptions` - Telemetry settings
- `ApplicationOptions` - General application settings

## Best Practices

1. **Never duplicate shared configuration** - If a setting applies to multiple services, put it in ServiceDefaults
2. **Use strongly-typed options** - Always access configuration via `IOptions<T>` injection
3. **Service-specific only** - Service appsettings should only contain service-unique settings
4. **Token replacement** - Use `#{Token.Name}` syntax for values that change per environment
5. **Sensitive data** - Never commit real passwords or secrets. Use user secrets or Azure Key Vault
6. **Command-line for operations** - Use command-line arguments for operational tasks (like migrations)

## Special Cases

### Console Applications (NetCore.Migration)

Console applications that use `Host.CreateDefaultBuilder()` instead of `WebApplication.CreateBuilder()` can still use shared configuration but may use **command-line arguments** for operational control instead of configuration files.

Example:
```bash
# NetCore.Migration uses CLI args instead of config
dotnet run --project NetCore.Migration -- -d -m -s
```

See service-specific README files for details.

## Troubleshooting

### NETSDK1152: Multiple publish output files

This error occurs when ServiceDefaults appsettings are copied to service output. Ensure:

```xml
<ProjectReference Include="..\NetCore\NetCore.ServiceDefaults\NetCore.ServiceDefaults.csproj">
  <PrivateAssets>contentfiles</PrivateAssets>
</ProjectReference>
```

### Configuration not loading

1. Verify `builder.AddServiceDefaults()` is called in Program.cs
2. Check file paths in Development mode
3. Verify embedded resources are included in ServiceDefaults.csproj

### Override not working

Remember the configuration hierarchy - service-specific settings override shared settings. Check the priority order above.
