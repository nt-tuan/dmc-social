FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["./Blogs/Blogs.csproj", "./Blogs/"]
COPY ["./RankingWorker/RankingWorker.csproj", "./RankingWorker/"]
RUN dotnet restore "./RankingWorker/RankingWorker.csproj"
COPY . .
WORKDIR "/src/RankingWorker/."
RUN dotnet build "RankingWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RankingWorker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RankingWorker.dll"]
