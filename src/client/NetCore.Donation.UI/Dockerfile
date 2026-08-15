#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
COPY ["src/Directory.Packages.props", "src/"]
COPY ["src/client/NetCore.Donation/NetCore.Donation.ServiceDefaults/NetCore.Donation.ServiceDefaults.csproj", "src/client/NetCore.Donation/NetCore.Donation.ServiceDefaults/"]
COPY ["src/client/NetCore.Donation.Api/NetCore.Donation.Api.csproj", "src/client/NetCore.Donation.Api/"]
COPY ["src/NetCore.Donation.Domain/NetCore.Donation.Domain.csproj", "src/NetCore.Donation.Domain/"]
COPY ["src/NetCore.Donation.Application/NetCore.Donation.Application.csproj", "src/NetCore.Donation.Application/"]
COPY ["src/NetCore.Donation.Infrastructure.Database/NetCore.Donation.Infrastructure.Database.csproj", "src/NetCore.Donation.Infrastructure.Database/"]
COPY ["src/client/NetCore.Donation.UI/NetCore.Donation.UI.csproj", "src/client/NetCore.Donation.UI/"]
RUN dotnet restore "src/client/NetCore.Donation.UI/NetCore.Donation.UI.csproj"
COPY . .

WORKDIR "/src/client/NetCore.Donation.UI"
RUN dotnet build "NetCore.Donation.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetCore.Donation.UI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetCore.Donation.UI.dll"]