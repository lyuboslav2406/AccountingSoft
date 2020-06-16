FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY . .
RUN dotnet restore --configfile "NuGet/nuget.config"
WORKDIR "/src/Backup.WebApi"
RUN dotnet build "Backup.WebApi.csproj" -c Release -o /app --no-restore

FROM build AS publish
RUN dotnet publish "Backup.WebApi.csproj" -c Release -o /app --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Backup.WebApi.dll"]