FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CoffeeSpace.ProductApi/CoffeeSpace.ProductApi.csproj", "CoffeeSpace.ProductApi/"]
COPY ["CoffeeSpace.Core/CoffeeSpace.Core.csproj", "CoffeeSpace.Core/"]
COPY ["CoffeeSpace.Domain/CoffeeSpace.Domain.csproj", "CoffeeSpace.Domain/"]
COPY ["CoffeeSpace.Messages/CoffeeSpace.Messages.csproj", "CoffeeSpace.Messages/"]
RUN dotnet restore "CoffeeSpace.ProductApi/CoffeeSpace.ProductApi.csproj"
COPY . .
WORKDIR "/src/CoffeeSpace.ProductApi"
RUN dotnet build "CoffeeSpace.ProductApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoffeeSpace.ProductApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoffeeSpace.ProductApi.dll"]
