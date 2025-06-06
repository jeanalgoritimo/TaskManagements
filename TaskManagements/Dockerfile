# Stage 1: base runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Stage 2: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solução e restaurar dependências
COPY UserProTasks.sln .
COPY UserproTasks.Application/UserproTasks.Application.csproj ./UserproTasks.Application/
COPY UserproTasks.Domain/UserproTasks.Domain.csproj ./UserproTasks.Domain/
COPY UserproTasks.Infrastructure/UserproTasks.Infrastructure.csproj ./UserproTasks.Infrastructure/
COPY UserProTasks.Api/UserProTasks.Api.csproj ./UserProTasks.Api/
COPY UserproTasks.Tests/UserproTasks.Tests.csproj ./UserproTasks.Tests/
RUN dotnet restore UserProTasks.sln

# Copiar todo código
COPY . .

# Buildar
WORKDIR /src/UserProTasks.Api
RUN dotnet build -c Release -o /app/build

# Publish para pasta /app/publish
FROM build AS publish
WORKDIR /src/UserProTasks.Api
RUN dotnet publish -c Release -o /app/publish

# Imagem final com runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserProTasks.Api.dll"]
