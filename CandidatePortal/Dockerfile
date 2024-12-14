# Use the ASP.NET Core 8 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5240

# Use the SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CandidatePortal/CandidatePortal.csproj", "./"]
RUN dotnet restore "./CandidatePortal.csproj"
COPY . .
WORKDIR "/src/CandidatePortal"
RUN dotnet build "CandidatePortal.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "CandidatePortal.csproj" -c Release -o /app/publish

# Use the runtime image for final deployment
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CandidatePortal.dll"]
