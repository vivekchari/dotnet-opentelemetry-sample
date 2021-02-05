FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /src
COPY *.csproj .

RUN dotnet restore

COPY . . 
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT [ "dotnet", "FirstApi.dll" ]