# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de projeto e restaura dependências
COPY Usuario.API/Usuario.Api.csproj Usuario.API/
COPY Usuario.Application/Usuario.Application.csproj Usuario.Application/
COPY Usuario.Domain/Usuario.Domain.csproj Usuario.Domain/
COPY Usuario.Infrastructure/Usuario.Infrastructure.csproj Usuario.Infrastructure/

RUN dotnet restore Usuario.API/Usuario.Api.csproj

# Copia todo o código e compila
COPY . .
WORKDIR /src/Usuario.API
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Usuario.Api.dll"]