# Build stage for VibeCheck (Blazor WebAssembly)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy the solution file and the VibeCheck and VibeCheckModel project files
COPY VibeCheck.sln ./
COPY VibeCheck/VibeCheck.csproj ./VibeCheck/
COPY VibeCheckModel/VibeCheckModel.csproj ./VibeCheckModel/
COPY VibeCheckServer/VibeCheckServer.csproj ./VibeCheckServer/

RUN dotnet restore

# Copy the remaining files and build the application
COPY . .
RUN dotnet publish VibeCheckServer/VibeCheckServer.csproj -c Release -o /app/publish/server
RUN dotnet publish VibeCheck/VibeCheck.csproj -c Release -o /app/publish/wwwroot

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copy the published outputs from both build stages
COPY --from=build /app/publish/server .
COPY --from=build /app/publish/wwwroot ./wwwroot


RUN ls -l /app/wwwroot/_framework

# Expose ports and start the app
EXPOSE 8080

ENTRYPOINT ["dotnet", "VibeCheckServer.dll"]