FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/OrderBook.WebApi/OrderBook.WebApi.csproj", "src/OrderBook.WebApi/"]
RUN dotnet restore "./src/OrderBook.WebApi/OrderBook.WebApi.csproj"
COPY . .
WORKDIR "/src/src/OrderBook.WebApi"
RUN dotnet build "./OrderBook.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OrderBook.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderBook.WebApi.dll"]