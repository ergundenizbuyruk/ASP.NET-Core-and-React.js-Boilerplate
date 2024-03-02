FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["aspnetcore/src/Pattern.API/Pattern.API.csproj", "src/Pattern.API/"]
COPY ["aspnetcore/src/Pattern.Application/Pattern.Application.csproj", "src/Pattern.Application/"]
COPY ["aspnetcore/src/Pattern.Persistence/Pattern.Persistence.csproj", "src/Pattern.Persistence/"]
COPY ["aspnetcore/src/Pattern.Core/Pattern.Core.csproj", "src/Pattern.Core/"]
RUN dotnet restore "./src/Pattern.API/./Pattern.API.csproj"
COPY ./aspnetcore .
WORKDIR "/src/src/Pattern.API"
RUN dotnet build "./Pattern.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Pattern.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pattern.API.dll"]