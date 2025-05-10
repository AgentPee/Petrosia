# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy and restore
COPY Petrosia/Petrosia.csproj ./  # Update path if needed
RUN dotnet restore

# Copy everything else
COPY Petrosia/ ./                 # Update path if needed
RUN dotnet publish -c Release -o /out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .
EXPOSE 80
ENTRYPOINT ["dotnet", "Petrosia.dll"]