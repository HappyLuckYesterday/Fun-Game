#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/Rhisis.Login/Rhisis.Login.csproj", "src/Rhisis.Login/"]
COPY ["src/Rhisis.Core/Rhisis.Core.csproj", "src/Rhisis.Core/"]
COPY ["src/Rhisis.Network/Rhisis.Network.csproj", "src/Rhisis.Network/"]
COPY ["src/Rhisis.Game.Abstractions/Rhisis.Game.Abstractions.csproj", "src/Rhisis.Game.Abstractions/"]
COPY ["src/Rhisis.Game.Common/Rhisis.Game.Common.csproj", "src/Rhisis.Game.Common/"]
COPY ["src/Rhisis.Database/Rhisis.Database.csproj", "src/Rhisis.Database/"]
RUN dotnet restore "src/Rhisis.Login/Rhisis.Login.csproj"
COPY . .
WORKDIR "/src/src/Rhisis.Login"
RUN dotnet build "Rhisis.Login.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rhisis.Login.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rhisis.Login.dll"]