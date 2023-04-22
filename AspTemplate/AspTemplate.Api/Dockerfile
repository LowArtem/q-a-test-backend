FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AspTemplate.Api/AspTemplate.Api.csproj", "AspTemplate.Api/"]
COPY ["AspTemplate.Data/AspTemplate.Data.csproj", "AspTemplate.Data/"]
COPY ["AspTemplate.Core/AspTemplate.Core.csproj", "AspTemplate.Core/"]
RUN dotnet restore "AspTemplate.Api/AspTemplate.Api.csproj"
COPY . .
WORKDIR "/src/AspTemplate.Api"
RUN dotnet build "AspTemplate.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AspTemplate.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AspTemplate.Api.dll"]
