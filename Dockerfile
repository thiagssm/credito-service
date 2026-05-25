FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CreditoService.sln global.json ./
COPY src/CreditoService.Domain/CreditoService.Domain.csproj src/CreditoService.Domain/
COPY src/CreditoService.Application/CreditoService.Application.csproj src/CreditoService.Application/
COPY src/CreditoService.Infrastructure/CreditoService.Infrastructure.csproj src/CreditoService.Infrastructure/
COPY src/CreditoService.Api/CreditoService.Api.csproj src/CreditoService.Api/
COPY tests/CreditoService.Tests/CreditoService.Tests.csproj tests/CreditoService.Tests/

RUN dotnet restore CreditoService.sln

COPY . .
RUN dotnet publish src/CreditoService.Api/CreditoService.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "CreditoService.Api.dll"]
