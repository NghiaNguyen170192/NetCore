#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/client/NetCore.Api/NetCore.Api.csproj", "src/client/NetCore.Api/"]
COPY ["src/application/NetCore.Application.Dtos/NetCore.Application.Dtos.csproj", "src/application/NetCore.Application.Dtos/"]
COPY ["src/application/NetCore.Application/NetCore.Application.csproj", "src/application/NetCore.Application/"]
COPY ["src/infrastructure/NetCore.Infrastructure.Database/NetCore.Infrastructure.Database.csproj", "src/infrastructure/NetCore.Infrastructure.Database/"]
COPY ["src/infrastructure/NetCore.Infrastructure.Database.Models/NetCore.Infrastructure.Database.Models.csproj", "src/infrastructure/NetCore.Infrastructure.Database.Models/"]
COPY ["src/shared/NetCore.Shared/NetCore.Shared.csproj", "src/shared/NetCore.Shared/"]

RUN dotnet restore "src/client/NetCore.Api/NetCore.Api.csproj"
COPY . .

RUN dotnet build "src/client/NetCore.Api/NetCore.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/client/NetCore.Api/NetCore.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetCore.Api.dll"]