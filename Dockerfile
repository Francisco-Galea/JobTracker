FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["JobTracker.API/JobTracker.API.csproj", "JobTracker.API/"]
COPY ["JobTracker.Application/JobTracker.Application.csproj", "JobTracker.Application/"]
COPY ["JobTracker.Domain/JobTracker.Domain.csproj", "JobTracker.Domain/"]
COPY ["JobTracker.Infrastructure/JobTracker.Infrastructure.csproj", "JobTracker.Infrastructure/"]
RUN dotnet restore "JobTracker.API/JobTracker.API.csproj"

COPY . .
WORKDIR "/src/JobTracker.API"
RUN dotnet build "JobTracker.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JobTracker.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobTracker.API.dll"]