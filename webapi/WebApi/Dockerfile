FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["webapi/WebApi/WebApi.csproj", "webapi/WebApi/"]
COPY ["webapi/Application/Application.csproj", "webapi/Application/"]
COPY ["webapi/Domain/Domain.csproj", "webapi/Domain/"]
COPY ["webapi/Infrastructure/Infrastructure.csproj", "webapi/Infrastructure/"]
RUN dotnet restore "webapi/WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/webapi/WebApi"
RUN dotnet build "WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SCISalesTest.WebApi.dll"]
