# NetCore
**Requisites** 
.Net Core 5.0

<br />

## To Run Application:
### Vs Code

```bash
cd .\docker\

docker-compose up -d

dotnet restore

dotnet build NetCore.sln
```

<br />


**Add application database migrations:**
```bash
cd .\src\Infrastructure\NetCore.Infrastructure.Database\

dotnet ef migrations add your_migration_name
```

<br />
