FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["OrderingApi/OrderingApi.csproj", "OrderingApi/"]
RUN dotnet restore "OrderingApi/OrderingApi.csproj"
COPY . .
WORKDIR "/src/OrderingApi"
RUN dotnet build "OrderingApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderingApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderingApi.dll"]
