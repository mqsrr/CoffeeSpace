FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CoffeeSpace.ApiGateway/CoffeeSpace.ApiGateway.csproj", "CoffeeSpace.ApiGateway/"]
RUN dotnet restore "CoffeeSpace.ApiGateway/CoffeeSpace.ApiGateway.csproj"
COPY . .
WORKDIR "/src/CoffeeSpace.ApiGateway"
RUN dotnet build "CoffeeSpace.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoffeeSpace.ApiGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoffeeSpace.ApiGateway.dll"]
