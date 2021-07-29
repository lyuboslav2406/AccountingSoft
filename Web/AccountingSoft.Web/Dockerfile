#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Web/AccountingSoft.Web/AccountingSoft.Web.csproj", "Web/AccountingSoft.Web/"]
COPY ["Data/AccountingSoft.Data/AccountingSoft.Data.csproj", "Data/AccountingSoft.Data/"]
COPY ["AccountingSoft.Common/AccountingSoft.Common.csproj", "AccountingSoft.Common/"]
COPY ["Data/AccountingSoft.Data.Common/AccountingSoft.Data.Common.csproj", "Data/AccountingSoft.Data.Common/"]
COPY ["Data/AccountingSoft.Data.Models/AccountingSoft.Data.Models.csproj", "Data/AccountingSoft.Data.Models/"]
COPY ["Services/AccountingSoft.Services.Mapping/AccountingSoft.Services.Mapping.csproj", "Services/AccountingSoft.Services.Mapping/"]
COPY ["Web/AccountingSoft.Web.ViewModels/AccountingSoft.Web.ViewModels.csproj", "Web/AccountingSoft.Web.ViewModels/"]
COPY ["Services/AccountingSoft.Services.Data/AccountingSoft.Services.Data.csproj", "Services/AccountingSoft.Services.Data/"]
COPY ["Services/AccountingSoft.Services.Messaging/AccountingSoft.Services.Messaging.csproj", "Services/AccountingSoft.Services.Messaging/"]
RUN dotnet restore "Web/AccountingSoft.Web/AccountingSoft.Web.csproj"
COPY . .
WORKDIR "/src/Web/AccountingSoft.Web"
RUN dotnet build "AccountingSoft.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AccountingSoft.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AccountingSoft.Web.dll"]