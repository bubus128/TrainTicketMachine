# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["TrainTicketMachine.Api/TrainTicketMachine.Api.csproj", "TrainTicketMachine.Api/"]
COPY ["TrainTicketMachine.Core/TrainTicketMachine.Core.csproj", "TrainTicketMachine.Core/"]
COPY ["TrainTicketMachine.Infrastructure/TrainTicketMachine.Infrastructure.csproj", "TrainTicketMachine.Infrastructure/"]
RUN dotnet restore "./TrainTicketMachine.Api/TrainTicketMachine.Api.csproj"
COPY . .
WORKDIR "/src/TrainTicketMachine.Api"
RUN dotnet build "./TrainTicketMachine.Api.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "./TrainTicketMachine.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrainTicketMachine.Api.dll"]