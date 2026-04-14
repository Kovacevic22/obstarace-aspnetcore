FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["ObstaRaceChat.API/ObstaRaceChat.API.csproj", "ObstaRaceChat.API/"]
COPY ["ObstaRaceChat.Application/ObstaRaceChat.Application.csproj", "ObstaRaceChat.Application/"]
COPY ["ObstaRaceChat.Infrastructure/ObstaRaceChat.Infrastructure.csproj", "ObstaRaceChat.Infrastructure/"]
COPY ["ObstaRaceChat.Domain/ObstaRaceChat.Domain.csproj", "ObstaRaceChat.Domain/"]

RUN dotnet restore "ObstaRaceChat.API/ObstaRaceChat.API.csproj"

COPY . .
WORKDIR "/src/ObstaRaceChat.API"
RUN dotnet publish "ObstaRaceChat.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ObstaRaceChat.API.dll"]