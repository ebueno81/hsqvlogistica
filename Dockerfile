
# ========================================
# ETAPA 1: COMPILACION
# ========================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copiar archivo del proyecto
COPY ["HsqvLogistica.csproj", "./"]

# Restaurar paquetes NuGet
RUN dotnet restore "HsqvLogistica.csproj"

# Copiar el resto del codigo
COPY . .

# Publicar en modo Release
RUN dotnet publish "HsqvLogistica.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

# ========================================
# ETAPA 2: EJECUCION
# ========================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

# Puerto HTTP dentro del contenedor
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

# Copiar archivos publicados
COPY --from=build /app/publish .

# Iniciar aplicacion
ENTRYPOINT ["dotnet", "HsqvLogistica.dll"]
