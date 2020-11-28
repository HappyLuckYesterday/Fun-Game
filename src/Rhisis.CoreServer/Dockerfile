#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Rhisis.CoreServer/Rhisis.CoreServer.csproj", "src/Rhisis.CoreServer/"]
COPY ["src/Rhisis.Messaging.RabbitMQ/Rhisis.Messaging.RabbitMQ.csproj", "src/Rhisis.Messaging.RabbitMQ/"]
COPY ["src/Rhisis.Messaging.Abstractions/Rhisis.Messaging.Abstractions.csproj", "src/Rhisis.Messaging.Abstractions/"]
COPY ["src/Rhisis.Core/Rhisis.Core.csproj", "src/Rhisis.Core/"]
RUN dotnet restore "src/Rhisis.CoreServer/Rhisis.CoreServer.csproj"
COPY . .
WORKDIR "/src/src/Rhisis.CoreServer"
RUN dotnet build "Rhisis.CoreServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rhisis.CoreServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rhisis.CoreServer.dll"]