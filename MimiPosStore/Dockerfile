# Base stage: ASP.NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 8080
EXPOSE 8081

# Build stage: SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files
COPY ["MimiPosStore/MimiPosStore.csproj", "MimiPosStore/"]
COPY ["BAL/BAL.csproj", "BAL/"]
COPY ["DAL/DAL.csproj", "DAL/"]
COPY ["SharedModels/SharedModels.csproj", "SharedModels/"]

# Restore dependencies
RUN dotnet restore "MimiPosStore/MimiPosStore.csproj"

# Copy all source code
COPY . .
WORKDIR "/src/MimiPosStore"
RUN dotnet build "MimiPosStore.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "MimiPosStore.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage: Runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MimiPosStore.dll"]
