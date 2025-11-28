FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore (fast cache)
COPY GKFashionAPI.csproj ./
RUN dotnet restore

# Copy everything else and publish
COPY . ./
RUN dotnet publish GKFashionAPI.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80

# Use Render's $PORT if provided, else default to 80
ENTRYPOINT ["bash", "-lc", "ASPNETCORE_URLS=http://0.0.0.0:${PORT:-80} dotnet GKFashionAPI.dll"]
