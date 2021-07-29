FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /AccountingSoft
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /AccountingSoft
COPY . .
RUN dotnet restore --configfile "nuget.config"
WORKDIR "/AccountingSoft/Web/AccountingSoft.Web/"
RUN dotnet build "AccountingSoft.Web.csproj" -c Release -o /AccountingSoft --no-restore

FROM build AS publish
RUN dotnet publish "AccountingSoft.Web.csproj" -c Release -o /AccountingSoft --no-restore

FROM base AS final
WORKDIR /AccountingSoft/Web/AccountingSoft.Web/
COPY --from=publish /AccountingSoft/Web/AccountingSoft.Web/ .
ENTRYPOINT ["dotnet", "AccountingSoft.Web.dll"]