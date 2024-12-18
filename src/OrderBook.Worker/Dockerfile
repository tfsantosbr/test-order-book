
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/OrderBook.Worker/OrderBook.Worker.csproj", "src/OrderBook.Worker/"]
COPY ["src/OrderBook.Infrastructure/OrderBook.Infrastructure.csproj", "src/OrderBook.Infrastructure/"]
COPY ["src/OrderBook.Application/OrderBook.Application.csproj", "src/OrderBook.Application/"]
RUN dotnet restore "./src/OrderBook.Worker/OrderBook.Worker.csproj"
COPY . .
WORKDIR "/src/src/OrderBook.Worker"
RUN dotnet build "./OrderBook.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OrderBook.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderBook.Worker.dll"]