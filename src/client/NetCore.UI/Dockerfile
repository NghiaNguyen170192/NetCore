#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
COPY ["src/client/NetCore.UI/NetCore.UI.csproj", "src/client/NetCore.UI/"]
RUN dotnet restore "src/client/NetCore.UI/NetCore.UI.csproj"
COPY . .

WORKDIR "/src/client/NetCore.UI"
RUN dotnet build "NetCore.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetCore.UI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetCore.UI.dll"]