#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Bandit.ACS.Daemon/Bandit.ACS.Daemon.csproj", "src/Bandit.ACS.Daemon/"]
COPY ["src/Bandit.ACS.MgdbRepository/Bandit.ACS.MgdbRepository.csproj", "src/Bandit.ACS.MgdbRepository/"]
COPY ["src/Bandit.ACS.NpgsqlRepository/Bandit.ACS.NpgsqlRepository.csproj", "src/Bandit.ACS.NpgsqlRepository/"]
RUN dotnet restore "src/Bandit.ACS.Daemon/Bandit.ACS.Daemon.csproj"
COPY . .
WORKDIR "/src/src/Bandit.ACS.Daemon"
RUN dotnet build "Bandit.ACS.Daemon.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bandit.ACS.Daemon.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bandit.ACS.Daemon.dll"]