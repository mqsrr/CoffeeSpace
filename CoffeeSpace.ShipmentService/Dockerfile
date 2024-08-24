FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CoffeeSpace.ShipmentService/CoffeeSpace.ShipmentService.csproj", "CoffeeSpace.ShipmentService/"]
COPY ["CoffeeSpace.Shared/CoffeeSpace.Shared.csproj", "CoffeeSpace.Shared/"]
COPY ["CoffeeSpace.Domain/CoffeeSpace.Domain.csproj", "CoffeeSpace.Domain/"]
COPY ["CoffeeSpace.Messages/CoffeeSpace.Messages.csproj", "CoffeeSpace.Messages/"]
RUN dotnet restore "CoffeeSpace.ShipmentService/CoffeeSpace.ShipmentService.csproj"
COPY . .
WORKDIR "/src/CoffeeSpace.ShipmentService"
RUN dotnet build "CoffeeSpace.ShipmentService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CoffeeSpace.ShipmentService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoffeeSpace.ShipmentService.dll"]
