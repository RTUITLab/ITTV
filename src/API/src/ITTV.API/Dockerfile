FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["API/src/ITTV.API/ITTV.API.csproj", "ITTV.API/"]
COPY ["API/src/ITTV.API.Infrastructure.Data/ITTV.API.Infrastructure.Data.csproj", "ITTV.API.Infrastructure.Data/"]
COPY ["API/src/ITTV.API.Domain/ITTV.API.Domain.csproj", "ITTV.API.Domain/"]
COPY ["API/src/ITTV.API.Core/ITTV.API.Core.csproj", "ITTV.API.Core/"]
RUN dotnet restore "API/src/ITTV.API/ITTV.API.csproj"
COPY . .
WORKDIR "/src/ITTV.API"
RUN dotnet build "ITTV.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ITTV.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ITTV.API.dll"]
