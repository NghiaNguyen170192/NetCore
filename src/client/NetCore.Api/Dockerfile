#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443	

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY ["src/shared/NetCore.Shared/NetCore.Shared.csproj", "src/shared/NetCore.Shared/"]
COPY ["src/application/NetCore.Application/NetCore.Application.csproj", "src/application/NetCore.Application/"]
COPY ["src/infrastructure/NetCore.Infrastructure.AuthenticationDatabase/NetCore.Infrastructure.AuthenticationDatabase.csproj", "src/infrastructure/NetCore.Infrastructure.AuthenticationDatabase/"]
COPY ["src/infrastructure/NetCore.Infrastructure.Database/NetCore.Infrastructure.Database.csproj", "src/infrastructure/NetCore.Infrastructure.Database/"]
COPY ["src/client/NetCore.Api/NetCore.Api.csproj", "src/client/NetCore.Api/"]
RUN dotnet restore "src/client/NetCore.Api/NetCore.Api.csproj"
COPY . .

WORKDIR "src/client/NetCore.Api"
RUN dotnet build "NetCore.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetCore.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetCore.Api.dll"]