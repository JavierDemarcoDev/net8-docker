FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5100
EXPOSE 5101

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Docker.API/Directory.Packages.props", "Docker.API/"]
COPY ["Directory.Packages.props", "."]
COPY ["Docker.API/Docker.API.csproj", "Docker.API/"]
RUN dotnet restore "./Docker.API/./Docker.API.csproj"
COPY . .
WORKDIR "/src/Docker.API"
RUN dotnet build "./Docker.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Docker.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Docker.API.dll"]