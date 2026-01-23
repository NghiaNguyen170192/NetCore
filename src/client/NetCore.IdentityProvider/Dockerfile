#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Directory.Packages.props", "src/"]
COPY ["src/client/NetCore.IdentityProvider/NetCore.IdentityProvider.csproj", "src/client/NetCore.IdentityProvider/"]
COPY ["src/NetCore.Infrastructure.Database/NetCore.Infrastructure.Database.csproj", "src/NetCore.Infrastructure.Database/"]
COPY ["src/NetCore.Infrastructure.AuthenticationDatabase/NetCore.Infrastructure.AuthenticationDatabase.csproj", "src/NetCore.Infrastructure.AuthenticationDatabase/"]
COPY ["src/NetCore.Domain/NetCore.Domain.csproj", "src/NetCore.Domain/"]

RUN dotnet restore "src/client/NetCore.IdentityProvider/NetCore.IdentityProvider.csproj"
COPY . .

RUN dotnet build "src/client/NetCore.IdentityProvider/NetCore.IdentityProvider.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/client/NetCore.IdentityProvider/NetCore.IdentityProvider.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetCore.IdentityProvider.dll"]