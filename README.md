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

## To Run Application locally:
### Vs Code

```bash
dotnet dev-certs https -ep .\certificates\.netcore-api\https\netcore-api.pfx -p your-certificate-password --trust

docker compose --env-file .\.env -f .\docker-compose.local.yml up -d
```

It will automatically create and apply migration to  local database.

Open [API Endpoint](https://localhost:6001/swagger/index.html) for accessing available API
<br />