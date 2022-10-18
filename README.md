# NetCore

[![.NET](https://github.com/NghiaNguyen170192/NetCore/actions/workflows/netcore-ci.yml/badge.svg)](https://github.com/NghiaNguyen170192/NetCore/actions/workflows/netcore-ci.yml)

## Requisites
.Net Core 6.0

IdentityServer 4

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

## To Run Application local:
### Vs Code

```bash
docker compose --env-file .\.env -f .\docker-compose.dev.yml up -d
```

It will automatically setup local database and apply migration to the database. 
<br />