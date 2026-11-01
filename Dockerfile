# =============================================================================
#  Ảnh chạy production cho HTNL Shop (multi-stage, chạy non-root)
# =============================================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy riêng csproj trước để tận dụng cache của Docker layer
COPY HTNLShop.csproj ./
RUN dotnet restore HTNLShop.csproj

COPY . ./
RUN dotnet publish HTNLShop.csproj -c Release -o /app/publish --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_NOLOGO=true

COPY --from=build /app/publish ./

# Chạy bằng user mặc định của ảnh aspnet (không phải root)
USER $APP_UID

EXPOSE 8080
ENTRYPOINT ["dotnet", "HTNLShop.dll"]
