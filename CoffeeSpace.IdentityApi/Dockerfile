FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CoffeeSpace.IdentityApi/CoffeeSpace.IdentityApi.csproj", "CoffeeSpace.IdentityApi/"]
COPY ["CoffeeSpace.Domain/CoffeeSpace.Domain.csproj", "CoffeeSpace.Domain/"]
COPY ["CoffeeSpace.Core/CoffeeSpace.Core.csproj", "CoffeeSpace.Core/"]
COPY ["CoffeeSpace.Messages/CoffeeSpace.Messages.csproj", "CoffeeSpace.Messages/"]
RUN dotnet restore "CoffeeSpace.IdentityApi/CoffeeSpace.IdentityApi.csproj"
COPY . .
WORKDIR "/src/CoffeeSpace.IdentityApi"
RUN dotnet build "CoffeeSpace.IdentityApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoffeeSpace.IdentityApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoffeeSpace.IdentityApi.dll"]
