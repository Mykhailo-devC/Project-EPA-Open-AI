#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EPA-WebAPI/EPA-WebAPI.csproj", "EPA-WebAPI/"]
COPY ["EPA.Engine.Repository/Epa.Engine.Repository.csproj", "EPA.Engine.Repository/"]
COPY ["Epa.Engine.DB/Epa.Engine.DB.csproj", "Epa.Engine.DB/"]
COPY ["Epa.Engine.Models/Epa.Engine.Models.csproj", "Epa.Engine.Models/"]
RUN dotnet restore "EPA-WebAPI/EPA-WebAPI.csproj"
COPY . .
WORKDIR "/src/EPA-WebAPI"
RUN dotnet build "EPA-WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EPA-WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EPA-WebAPI.dll"]