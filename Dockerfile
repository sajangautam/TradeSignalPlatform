# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app


# Copy csproj and restore dependencies
COPY TradeSignalManager.Api/*.csproj ./TradeSignalManager.Api/
COPY TradeSignalManager.Application/*.csproj ./TradeSignalManager.Application/
COPY TradeSignalManager.Core/*.csproj ./TradeSignalManager.Core/
COPY TradeSignalManager.Infrastructure/*.csproj ./TradeSignalManager.Infrastructure/
COPY TradeSignalManager.Infrastructure/Data/sp500.json ./TradeSignalManager.Infrastructure/Data/sp500.json


RUN dotnet restore TradeSignalManager.Api/TradeSignalManager.Api.csproj

# Copy everything else and build
COPY . .

WORKDIR /app/TradeSignalManager.Api
RUN dotnet publish -c Release -o out

# Stage 2: Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/TradeSignalManager.Api/out ./
COPY --from=build /app/TradeSignalManager.Infrastructure/Data/sp500.json ./TradeSignalManager.Infrastructure/Data/sp500.json


EXPOSE 8080
ENTRYPOINT ["dotnet", "TradeSignalManager.Api.dll"]
