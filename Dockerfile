FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["./dmc-social/dmc-social.csproj", "./"]
RUN dotnet restore "./dmc-social.csproj"
COPY ./dmc-social/. .
WORKDIR "/src/."
RUN dotnet build "dmc-social.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dmc-social.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dmc-social.dll"]
