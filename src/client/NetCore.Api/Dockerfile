#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443	

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY ["src/NetCore.Domain/NetCore.Domain.csproj", "src/NetCore.Domain/"]
COPY ["src/NetCore.Application/NetCore.Application.csproj", "src/NetCore.Application/"]
COPY ["src/NetCore.Infrastructure.Database/NetCore.Infrastructure.Database.csproj", "src/NetCore.Infrastructure.Database/"]
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