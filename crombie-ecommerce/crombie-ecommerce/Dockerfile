# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["crombie-ecommerce/crombie-ecommerce/crombie-ecommerce.csproj", "crombie-ecommerce/"]
COPY ["crombie-ecommerce/crombie-ecommerce.BusinessLogic/crombie-ecommerce.BusinessLogic.csproj", "crombie-ecommerce.BusinessLogic/"]
COPY ["crombie-ecommerce/crombie-ecommerce.DataAccess/crombie-ecommerce.DataAccess.csproj", "crombie-ecommerce.DataAccess/"]
COPY ["crombie-ecommerce/crombie-ecommerce.Models/crombie-ecommerce.Models.csproj", "crombie-ecommerce.Models/"]

RUN dotnet restore "./crombie-ecommerce/crombie-ecommerce.csproj"

COPY . .

WORKDIR "/src/crombie-ecommerce/crombie-ecommerce"
RUN dotnet build "crombie-ecommerce.csproj" -c Release -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
RUN dotnet publish "./crombie-ecommerce.csproj" -c Release -o /app/publish

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "crombie-ecommerce.dll"]