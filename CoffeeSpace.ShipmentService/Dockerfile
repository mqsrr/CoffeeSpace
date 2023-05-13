FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CoffeeSpace.ShipmentService/CoffeeSpace.ShipmentService.csproj", "CoffeeSpace.ShipmentService/"]
COPY ["CoffeeSpace.Messages/CoffeeSpace.Messages.csproj", "CoffeeSpace.Messages/"]
COPY ["CoffeeSpace.Domain/CoffeeSpace.Domain.csproj", "CoffeeSpace.Domain/"]
COPY ["CoffeeSpace.Application/CoffeeSpace.Application.csproj", "CoffeeSpace.Application/"]
RUN dotnet restore "CoffeeSpace.ShipmentService/CoffeeSpace.ShipmentService.csproj"
COPY . .
WORKDIR "/src/CoffeeSpace.ShipmentService"
RUN dotnet build "CoffeeSpace.ShipmentService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoffeeSpace.ShipmentService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoffeeSpace.ShipmentService.dll"]
