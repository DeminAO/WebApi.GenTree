#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebApi.GenTree/WebApi.GenTree.csproj", "WebApi.GenTree/"]
RUN dotnet restore "./WebApi.GenTree/WebApi.GenTree.csproj"
COPY . .
WORKDIR "/src/WebApi.GenTree"
RUN dotnet build "./WebApi.GenTree.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApi.GenTree.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

ENV ConnectionStrings__DefaultConnection=Host=localhost;Port=5432;Database=NetrikaDb;Username=postgres;Password=test

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.GenTree.dll"]