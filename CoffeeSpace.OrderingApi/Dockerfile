FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CoffeeSpace.OrderingApi/CoffeeSpace.OrderingApi.csproj", "CoffeeSpace.OrderingApi/"]
COPY ["CoffeeSpace.Application/CoffeeSpace.Application.csproj", "CoffeeSpace.Application/"]
COPY ["CoffeeSpace.Domain/CoffeeSpace.Domain.csproj", "CoffeeSpace.Domain/"]
COPY ["CoffeeSpace.Messages/CoffeeSpace.Messages.csproj", "CoffeeSpace.Messages/"]
RUN dotnet restore "CoffeeSpace.OrderingApi/CoffeeSpace.OrderingApi.csproj"
COPY . .
WORKDIR "/src/CoffeeSpace.OrderingApi"
RUN dotnet build "CoffeeSpace.OrderingApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoffeeSpace.OrderingApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoffeeSpace.OrderingApi.dll"]
