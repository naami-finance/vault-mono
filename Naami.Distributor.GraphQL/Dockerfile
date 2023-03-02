FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Naami.Distributor.GraphQL/Naami.Distributor.GraphQL.csproj", "Naami.Distributor.GraphQL/"]
COPY ["Naami.Distributor.SDK/Naami.Distributor.SDK.csproj", "Naami.Distributor.SDK/"]
COPY ["Naami.Distributor.Data/Naami.Distributor.Data.csproj", "Naami.Distributor.Data/"]
RUN dotnet restore "Naami.Distributor.GraphQL/Naami.Distributor.GraphQL.csproj"
COPY . .
WORKDIR "/src/Naami.Distributor.GraphQL"
RUN dotnet build "Naami.Distributor.GraphQL.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Naami.Distributor.GraphQL.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Naami.Distributor.GraphQL.dll"]