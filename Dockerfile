# ----- Build Stage -----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /build

# Copy project files for restore
COPY ["src/AppointmentApplication.API/AppointmentApplication.API.csproj", "src/AppointmentApplication.API/"]
COPY ["src/AppointmentApplication.Application/AppointmentApplication.Application.csproj", "src/AppointmentApplication.Application/"]
COPY ["src/AppointmentApplication.Domain/AppointmentApplication.Domain.csproj", "src/AppointmentApplication.Domain/"]
COPY ["src/AppointmentApplication.Contracts/AppointmentApplication.Contracts.csproj", "src/AppointmentApplication.Contracts/"]
COPY ["src/AppointmentApplication.Infrastructure/AppointmentApplication.Infrastructure.csproj", "src/AppointmentApplication.Infrastructure/"]
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]

# Restore dependencies (only once)
RUN dotnet restore "src/AppointmentApplication.API/AppointmentApplication.API.csproj"

# Copy all source code
COPY . .

# Build and publish
RUN dotnet publish "src/AppointmentApplication.API/AppointmentApplication.API.csproj" -c Release -o /app

# ----- Final Stage -----
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

# Install timezone data for TimeZoneInfo support
RUN apt-get update && apt-get install -y tzdata && \
    ln -fs /usr/share/zoneinfo/America/Montreal /etc/localtime && \
    dpkg-reconfigure -f noninteractive tzdata && \
    rm -rf /var/lib/apt/lists/*

ENV TZ=America/Montreal

WORKDIR /app
COPY --from=build /app .

EXPOSE 80
ENTRYPOINT ["dotnet", "AppointmentApplication.API.dll"]
