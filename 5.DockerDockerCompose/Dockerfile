# Используем .NET SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем файл решения
COPY *.sln . 

# Копируем файл проекта
COPY Otus.Teaching.PromoCodeFactory.WebHost/*.csproj ./Otus.Teaching.PromoCodeFactory.WebHost/
COPY Otus.Teaching.PromoCodeFactory.DataAccess/*.csproj ./Otus.Teaching.PromoCodeFactory.DataAccess/
COPY Otus.Teaching.PromoCodeFactory.Core/*.csproj ./Otus.Teaching.PromoCodeFactory.Core/

# Восстанавливаем зависимости
RUN dotnet restore

# Копируем все остальные файлы
COPY . .

# Собираем проект
WORKDIR /app/Otus.Teaching.PromoCodeFactory.WebHost
RUN dotnet publish -c Release -o /app/out

# Используем ASP.NET Runtime для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Otus.Teaching.PromoCodeFactory.WebHost.dll"]