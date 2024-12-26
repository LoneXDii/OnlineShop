FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_ENVIRONMENT=Release

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/SupportService/
COPY ["SupportService.API/SupportService.API.csproj", "SupportService.API/"]
COPY ["SupportService.Application/SupportService.Application.csproj", "SupportService.Application/"]
COPY ["SupportService.Infrastructure/SupportService.Infrastructure.csproj", "SupportService.Infrastructure/"]
COPY ["SupportService.Domain/SupportService.Domain.csproj", "SupportService.Domain/"]
RUN dotnet restore "SupportService.API/SupportService.API.csproj"
COPY . .
WORKDIR "/src/SupportService/SupportService.API"
RUN dotnet build "SupportService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SupportService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SupportService.API.dll"]