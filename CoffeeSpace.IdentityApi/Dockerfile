FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CoffeeSpace.IdentityApi/CoffeeSpace.IdentityApi.csproj", "CoffeeSpace.IdentityApi/"]
COPY ["CoffeeSpace.Messages/CoffeeSpace.Messages.csproj", "CoffeeSpace.Messages/"]
COPY ["CoffeeSpace.Domain/CoffeeSpace.Domain.csproj", "CoffeeSpace.Domain/"]
COPY ["CoffeeSpace.Shared/CoffeeSpace.Shared.csproj", "CoffeeSpace.Shared/"]
RUN dotnet restore "CoffeeSpace.IdentityApi/CoffeeSpace.IdentityApi.csproj"
COPY . .
WORKDIR "/src/CoffeeSpace.IdentityApi"
RUN dotnet build "CoffeeSpace.IdentityApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CoffeeSpace.IdentityApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoffeeSpace.IdentityApi.dll"]
