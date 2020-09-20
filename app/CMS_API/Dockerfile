#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["app/CMS_API/CMS_API.csproj", "app/CMS_API/"]
COPY ["lib/DataService/DataService.csproj", "lib/DataService/"]
COPY ["lib/Common/Common.csproj", "lib/Common/"]
COPY ["lib/Tools/Tools.csproj", "lib/Tools/"]
RUN dotnet restore "app/CMS_API/CMS_API.csproj"
COPY . .
WORKDIR "/src/app/CMS_API"
RUN dotnet build "CMS_API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CMS_API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CMS_API.dll"]