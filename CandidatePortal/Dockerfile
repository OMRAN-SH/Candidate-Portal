# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy and restore dependencies
COPY ./CandidatePortal.csproj /app/  
RUN dotnet restore

# Copy the rest of the code and build the app
COPY . ./  
RUN dotnet publish -c Release -o out

# Debugging: List files in the build output directory
RUN ls -R /app/out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Copy the published app from the build stage
COPY --from=build /app/out ./

# Debugging: List files in the runtime app directory
RUN ls -R /app

# Expose the HTTP port and set the entry point
EXPOSE 80
ENTRYPOINT ["dotnet", "CandidatePortal.dll"]

# Install EF Core tools
RUN apt-get update \
    && apt-get install -y wget \
    && wget https://packages.microsoft.com/keys/microsoft.asc -O /etc/apt/trusted.gpg.d/microsoft.asc \
    && wget https://packages.microsoft.com/config/ubuntu/20.04/prod.list -O /etc/apt/sources.list.d/dotnetdev.list \
    && apt-get update \
    && apt-get install -y dotnet-sdk-8.0

RUN dotnet tool install --global dotnet-ef

# Add the .NET tools to the PATH
ENV PATH="${PATH}:/root/.dotnet/tools"

