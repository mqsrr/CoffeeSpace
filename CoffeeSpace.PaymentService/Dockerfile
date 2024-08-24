FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CoffeeSpace.PaymentService/CoffeeSpace.PaymentService.csproj", "CoffeeSpace.PaymentService/"]
COPY ["CoffeeSpace.Shared/CoffeeSpace.Shared.csproj", "CoffeeSpace.Shared/"]
COPY ["CoffeeSpace.Domain/CoffeeSpace.Domain.csproj", "CoffeeSpace.Domain/"]
COPY ["CoffeeSpace.Messages/CoffeeSpace.Messages.csproj", "CoffeeSpace.Messages/"]
RUN dotnet restore "CoffeeSpace.PaymentService/CoffeeSpace.PaymentService.csproj"
COPY . .
WORKDIR "/src/CoffeeSpace.PaymentService"
RUN dotnet build "CoffeeSpace.PaymentService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CoffeeSpace.PaymentService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoffeeSpace.PaymentService.dll"]
