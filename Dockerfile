# =========================
# Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore "Tickets.API/Tickets.API.csproj"

RUN dotnet publish "Tickets.API/Tickets.API.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore


# =========================
# Runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Tickets.API.dll"]