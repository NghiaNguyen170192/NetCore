#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/client/NetCore.Migration/NetCore.Migration.csproj", "src/client/NetCore.Migration/"]
COPY ["src/infrastructure/NetCore.Infrastructure.Database/NetCore.Infrastructure.Database.csproj", "src/infrastructure/NetCore.Infrastructure.Database/"]
COPY ["src/infrastructure/NetCore.Infrastructure.Database.Models/NetCore.Infrastructure.Database.Models.csproj", "src/infrastructure/NetCore.Infrastructure.Database.Models/"]
COPY ["src/infrastructure/NetCore.Infrastructure.AuthenticationDatabase/NetCore.Infrastructure.AuthenticationDatabase.csproj", "src/infrastructure/NetCore.Infrastructure.AuthenticationDatabase/"]
COPY ["src/shared/NetCore.Shared/NetCore.Shared.csproj", "src/shared/NetCore.Shared/"]
RUN dotnet restore "src/client/NetCore.Migration/NetCore.Migration.csproj"
COPY . .

RUN dotnet build "src/client/NetCore.Migration/NetCore.Migration.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/client/NetCore.Migration/NetCore.Migration.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetCore.Migration.dll"]