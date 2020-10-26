# Specify base image.
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy everything and restore / publish the solution.
COPY . ./
RUN dotnet build ./FrostAura.Clients.Components/FrostAura.Clients.Components.csproj
#RUN dotnet build ./FrostAura.Services.Identity.Core.Tests/FrostAura.Services.Identity.Core.Tests.csproj
RUN dotnet publish ./FrostAura.Clients.Components/FrostAura.Clients.Components.csproj -c Release -o /app/out

# Build runtime image off the correct base.
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "FrostAura.Clients.Components.dll"]
EXPOSE 80