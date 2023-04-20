FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CoffeeSpace.Service.OrderItems/CoffeeSpace.Service.OrderItems.csproj", "CoffeeSpace.Service.OrderItems/"]
RUN dotnet restore "CoffeeSpace.Service.OrderItems/CoffeeSpace.Service.OrderItems.csproj"
COPY . .
WORKDIR "/src/CoffeeSpace.Service.OrderItems"
RUN dotnet build "CoffeeSpace.Service.OrderItems.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoffeeSpace.Service.OrderItems.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoffeeSpace.Service.OrderItems.dll"]
