# Set up base image to build the docker container and expose ports for our API
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80 
EXPOSE 443

# Copy project files into the container itself and build the project
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ApartmentFinder.API/ApartmentFinder.API.csproj", "ApartmentFinder.API/"]
RUN dotnet restore "ApartmentFinder.API/ApartmentFinder.API.csproj"
COPY . .
WORKDIR "/src/ApartmentFinder.API"
RUN dotnet build "ApartmentFinder.API.csproj" -c Release -o /app/build

# Publish the project
FROM build AS publish
RUN dotnet publish "ApartmentFinder.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Set up the final image, copy the published application from the publish image and specify the entry point for running the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApartmentFinder.API.dll"]