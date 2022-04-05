FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ITTV.API/ITTV.API.csproj", "ITTV.API/"]
COPY ["ITTV.API.Core/ITTV.API.Core.csproj", "ITTV.API.Core/"]
COPY ["ITTV.API.Infrastructure.Data/ITTV.API.Infrastructure.Data.csproj", "ITTV.API.Infrastructure.Data/"]
COPY ["ITTV.API.Domain/ITTV.API.Domain.csproj", "ITTV.API.Domain/"]
RUN dotnet restore "ITTV.API/ITTV.API.csproj"
COPY . .
WORKDIR "/src/ITTV.API"
RUN dotnet build "ITTV.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ITTV.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ITTV.API.dll"]
