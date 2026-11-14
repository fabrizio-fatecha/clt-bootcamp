FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

COPY Directory.Build.props Directory.Packages.props ./
COPY src/ProductsCqrs.Domain/ProductsCqrs.Domain.csproj src/ProductsCqrs.Domain/
COPY src/ProductsCqrs.Application/ProductsCqrs.Application.csproj src/ProductsCqrs.Application/
COPY src/ProductsCqrs.Infrastructure/ProductsCqrs.Infrastructure.csproj src/ProductsCqrs.Infrastructure/
COPY src/ProductsCqrs.Api/ProductsCqrs.Api.csproj src/ProductsCqrs.Api/
RUN dotnet restore src/ProductsCqrs.Api/ProductsCqrs.Api.csproj

COPY src ./src
RUN dotnet publish src/ProductsCqrs.Api/ProductsCqrs.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet", "ProductsCqrs.Api.dll"]
