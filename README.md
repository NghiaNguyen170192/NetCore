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


**Add application database migration**
```bash
cd .\src\Infrastructure\NetCore.Infrastructure.Database\

dotnet ef migrations add InittialDatabase --context DatabaseContext -o .\Migrations\
```

<br />


**Add Identity Server Store migration (one time only)**
```bash
cd .\src\Infrastructure\NetCore.Infrastructure.Database\

dotnet ef migrations add InittialDatabase --context ConfigurationDbContext -o .\Migrations\ConfigurationDb

dotnet ef migrations add InittialDatabase --context PersistedGrantDbContext -o .\Migrations\PersistedGrantDb
```

<br />
