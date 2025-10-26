FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Transferencia.API/Transferencia.Api.csproj Transferencia.API/
COPY Transferencia.Application/Transferencia.Application.csproj Transferencia.Application/
COPY Transferencia.Domain/Transferencia.Domain.csproj Transferencia.Domain/
COPY Transferencia.Infrastructure/Transferencia.Infrastructure.csproj Transferencia.Infrastructure/

RUN dotnet restore Transferencia.API/Transferencia.Api.csproj

COPY . .
WORKDIR /src/Transferencia.API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Transferencia.Api.dll"]