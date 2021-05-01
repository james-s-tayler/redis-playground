FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app
COPY . .
WORKDIR /app
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build --no-restore -c Release -o output

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o output

FROM base AS final
WORKDIR /app
RUN ls -ltr
COPY --from=publish /app/output .
ENTRYPOINT ["dotnet", "Redis.Playground.Api.dll"]

