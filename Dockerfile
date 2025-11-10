# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de projeto (.csproj) e restaura dependências
COPY src/ContaCorrente.Api/*.csproj ContaCorrente.Api/
COPY src/ContaCorrente.Application/*.csproj ContaCorrente.Application/
COPY src/ContaCorrente.Domain/*.csproj ContaCorrente.Domain/
COPY src/ContaCorrente.Infra/*.csproj ContaCorrente.Infra/
RUN dotnet restore ContaCorrente.Api/ContaCorrente.Api.csproj

# Copia todo o código restante
COPY src/ .

WORKDIR /src/ContaCorrente.Api
RUN dotnet publish -c Release -o /app/out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "ContaCorrente.Api.dll"]
