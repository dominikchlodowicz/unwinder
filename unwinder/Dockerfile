#TODO: run when angular is fixed

# Use the official .NET Core 7 SDK image from Microsoft
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

# Set the working directory in the container
WORKDIR /app

# Install Node.js and npm
RUN apt-get update && apt-get install -y nodejs npm

# Install Angular CLI (this will install the latest version by default)
RUN npm install -g @angular/cli

# Copy the csproj file and restore any dependencies (via nuget)
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application code
COPY . ./

# Publish the application to /out
RUN dotnet publish -c Release -o out

# Use the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0

# Set working directory
WORKDIR /app

# Copy compiled application from build-env container
COPY --from=build-env /app/out .

# Expose port for the application
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "unwinder.dll"]

