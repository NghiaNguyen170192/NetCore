# NetCore

[![.NET](https://github.com/NghiaNguyen170192/NetCore/actions/workflows/netcore-ci.yml/badge.svg)](https://github.com/NghiaNguyen170192/NetCore/actions/workflows/netcore-ci.yml)

## About
An on going project that using .Net Core 6.
Purpose of this project is to learn  and Clean Architecture
- Domain Driven Design
- Clean Architecture
- Docker
- CI/CD

<br />


**Add application database migration**
```bash
cd .\src\NetCore.Infrastructure.Database\

dotnet ef migrations add migration_name --context ApplicationDatabaseContext -o .\Migrations\
```

<br />


**Add Identity Server Store migration (one time only)**
```bash
cd .\src\NetCore.Infrastructure.Database\

dotnet ef migrations add migration_name --context ConfigurationDbContext -o .\Migrations\ConfigurationDb

dotnet ef migrations add migration_name --context PersistedGrantDbContext -o .\Migrations\PersistedGrantDb
```

<br />

## To build NetCore on local Docker:
```bash
dotnet dev-certs https -ep .\certificates\.netcore-api\https\netcore-api.pfx -p aJ3oPVRd6vPWndrqSf4gYFsc5P3BYM --trust

docker compose --env-file .\.env -f .\docker-compose.local.yml build
```


## To run NetCore on local Docker:
```bash
docker compose --env-file .\.env -f .\docker-compose.local.yml up -d
```

Open [API Endpoint](https://localhost:6001/swagger/index.html) for accessing available API
<br />