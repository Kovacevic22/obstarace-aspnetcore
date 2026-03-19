FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["ObstaRace.API/ObstaRace.API.csproj", "ObstaRace.API/"]
COPY ["ObstaRace.Application/ObstaRace.Application.csproj", "ObstaRace.Application/"]
COPY ["ObstaRace.Infrastructure/ObstaRace.Infrastructure.csproj", "ObstaRace.Infrastructure/"]
COPY ["ObstaRace.Domain/ObstaRace.Domain.csproj", "ObstaRace.Domain/"]

RUN dotnet restore "ObstaRace.API/ObstaRace.API.csproj"

COPY . .
WORKDIR "/src/ObstaRace.API"
RUN dotnet publish "ObstaRace.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ObstaRace.API.dll"]