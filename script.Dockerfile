FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["./dmc-social/dmc-social.csproj", "./dmc-social/"]
COPY ["./scripts/scripts.csproj", "./scripts/"]
RUN dotnet restore "./scripts/scripts.csproj"
COPY . .
WORKDIR "/src/scripts/."
RUN dotnet build "scripts.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "scripts.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "scripts.dll"]
