# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the project file and restore dependencies
COPY Petrosia/Petrosia.csproj Petrosia/
RUN dotnet restore Petrosia/Petrosia.csproj

# Copy the rest of the app and build it
COPY . .
WORKDIR /src/Petrosia
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Open the default port
EXPOSE 80

# Set the entry point
ENTRYPOINT ["dotnet", "Petrosia.dll"]
