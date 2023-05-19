FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CoffeeSpace.PaymentService/CoffeeSpace.PaymentService.csproj", "CoffeeSpace.PaymentService/"]
COPY ["CoffeeSpace.Domain/CoffeeSpace.Domain.csproj", "CoffeeSpace.Domain/"]
COPY ["CoffeeSpace.Messages/CoffeeSpace.Messages.csproj", "CoffeeSpace.Messages/"]
COPY ["CoffeeSpace.Core/CoffeeSpace.Core.csproj", "CoffeeSpace.Core/"]
RUN dotnet restore "CoffeeSpace.PaymentService/CoffeeSpace.PaymentService.csproj"
COPY . .
WORKDIR "/src/CoffeeSpace.PaymentService"
RUN dotnet build "CoffeeSpace.PaymentService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoffeeSpace.PaymentService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoffeeSpace.PaymentService.dll"]
