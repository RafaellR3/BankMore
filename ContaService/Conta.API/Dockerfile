FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Conta.API/Conta.Api.csproj Conta.API/
COPY Conta.Application/Conta.Application.csproj Conta.Application/
COPY Conta.Domain/Conta.Domain.csproj Conta.Domain/
COPY Conta.Infrastructure/Conta.Infrastructure.csproj Conta.Infrastructure/

RUN dotnet restore Conta.API/Conta.Api.csproj

COPY . .
WORKDIR /src/Conta.API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Conta.Api.dll"]