# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src

# Copia os arquivos de projeto e restaura dependências
COPY ./src/ContaCorrente.Api/*.csproj ./ContaCorrente.Api/
COPY ./src/ContaCorrente.Application/*.csproj ./ContaCorrente.Application/
COPY ./src/ContaCorrente.Domain/*.csproj ./ContaCorrente.Domain/
COPY ./src/ContaCorrente.Infra/*.csproj ./ContaCorrente.Infra/

RUN dotnet restore ./ContaCorrente.Api/ContaCorrente.Api.csproj

# Copia o restante do código
COPY ./src ./src

WORKDIR /src/ContaCorrente.Api

RUN dotnet publish -c Release -o /app/out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

# Copia o build da etapa anterior
COPY --from=build /app/out ./

# Cria a pasta database
RUN mkdir -p /app/database

# Expondo a porta padrão do Kestrel
EXPOSE 5000

ENTRYPOINT ["dotnet", "ContaCorrente.Api.dll"]