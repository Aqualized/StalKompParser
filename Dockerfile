# Этот этап используется при запуске из VS в быстром режиме
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StalKompParser.csproj", "."]
RUN dotnet restore "./StalKompParser.csproj"
COPY . .
WORKDIR "/src/."
USER root
RUN dotnet build "./StalKompParser.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./StalKompParser.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StalKompParser.dll"]
