FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 3434
ENV ASPNETCORE_URLS=http://+:3434

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AuthApi.csproj", "src/"]
RUN dotnet restore "src/AuthApi.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "AuthApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthApi.dll"]
