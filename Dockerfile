# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy the solution file and all project files
COPY *.sln .
COPY VibeCheckServer/*.csproj ./VibeCheckServer/
COPY VibeCheckModel/*.csproj ./VibeCheckModel/
COPY VibeCheck/*.csproj ./VibeCheck/

RUN dotnet restore

# Copy the remaining files and build the application
COPY . .
WORKDIR /app/VibeCheck
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port and start the app
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "VibeCheck.dll"]