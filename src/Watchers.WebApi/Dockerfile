FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Watchers.WebApi/Watchers.WebApi.csproj", "Watchers.WebApi/"]
RUN dotnet restore "src/Watchers.WebApi/Watchers.WebApi.csproj"
COPY . .
WORKDIR "/src/Watchers.WebApi"
RUN dotnet build "Watchers.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Watchers.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Watchers.WebApi.dll"]
