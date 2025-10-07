# UsersApp

Многослойное ASP.NET Core MVC приложение для управления пользователями с соблюдением принципов SOLID.

## 🚀 Быстрый запуск

### Предварительные требования
- **.NET 8.0 SDK** ([скачать](https://dotnet.microsoft.com/download))
- **SQLite** (встроен в .NET)
- **Git** (для клонирования репозитория)

### Установка .NET на Linux
```bash
# Ubuntu/Debian
sudo apt update
sudo apt install -y dotnet-sdk-8.0

# Или через snap
sudo snap install dotnet-sdk --classic

# Проверка установки
dotnet --version
```

### Установка и запуск

#### 1. Клонирование репозитория
```bash
git clone <repository-url>
cd test-backend
```

#### 2. Восстановление зависимостей
```bash
# Восстановление всех пакетов NuGet
dotnet restore

# Или для конкретного проекта
dotnet restore UsersApp.Web/UsersApp.Web.csproj
```

#### 3. Создание/обновление базы данных
```bash
# Создание миграций (если нужно)
dotnet ef migrations add InitialCreate --project UsersApp.Infrastructure --startup-project UsersApp.Web

# Применение миграций к базе данных
dotnet ef database update --project UsersApp.Infrastructure --startup-project UsersApp.Web
```

#### 4. Запуск приложения

##### Способ 1: Стандартный запуск
```bash
# Запуск из корневой папки
dotnet run --project UsersApp.Web

# Или из папки проекта
cd UsersApp.Web
dotnet run
```

##### Способ 2: Запуск с указанием URL
```bash
# Запуск на другом порту
dotnet run --project UsersApp.Web --urls "http://localhost:5009"

# Запуск на всех интерфейсах
dotnet run --project UsersApp.Web --urls "http://0.0.0.0:5008"
```

##### Способ 3: Запуск через профили
```bash
# HTTP профиль (только HTTP)
dotnet run --project UsersApp.Web --launch-profile http

# HTTPS профиль (HTTP + HTTPS)
dotnet run --project UsersApp.Web --launch-profile https
```

### 🌐 Доступные интерфейсы

После запуска приложение будет доступно по следующим адресам:

| Интерфейс | URL | Описание |
|-----------|-----|----------|
| **🌐 Веб-интерфейс (MVC)** | http://localhost:5008 | Основной пользовательский интерфейс |
| **📚 Swagger API** | http://localhost:5008/swagger | Документация и тестирование API |
| **🔍 Поиск пользователей** | http://localhost:5008/Users?q=term | Поиск по имени или email |
| **🔒 HTTPS (если настроен)** | https://localhost:7118 | Безопасное соединение |

### 🔧 Настройка хоста и портов

#### Через переменные окружения
```bash
# Установка URL через переменную окружения
export ASPNETCORE_URLS="http://localhost:5009"
dotnet run --project UsersApp.Web

# Или в одной команде
ASPNETCORE_URLS="http://0.0.0.0:5008" dotnet run --project UsersApp.Web
```

#### Через конфигурацию
Создайте файл `appsettings.Production.json`:
```json
{
  "Urls": "http://0.0.0.0:5008;https://0.0.0.0:7118"
}
```

#### Через launchSettings.json
Измените `Properties/launchSettings.json`:
```json
{
  "profiles": {
    "custom": {
      "commandName": "Project",
      "applicationUrl": "http://localhost:5009",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

### 🐛 Решение проблем

#### Порт уже используется
```bash
# Найти процесс, использующий порт
lsof -i :5008

# Остановить процесс
kill <PID>

# Или запустить на другом порту
dotnet run --project UsersApp.Web --urls "http://localhost:5009"
```

#### Проблемы с базой данных
```bash
# Удалить базу данных и пересоздать
rm UsersApp.Web/users.db*
dotnet ef database update --project UsersApp.Infrastructure --startup-project UsersApp.Web
```

#### Проблемы с зависимостями
```bash
# Очистить кэш NuGet
dotnet nuget locals all --clear

# Пересобрать проект
dotnet clean
dotnet restore
dotnet build
```

## 🔧 Конфигурация хоста в коде

### Как определяется хост в ASP.NET Core:

#### 1. **launchSettings.json** (разработка)
```json
{
  "profiles": {
    "http": {
      "applicationUrl": "http://localhost:5008"
    },
    "https": {
      "applicationUrl": "https://localhost:7118;http://localhost:5008"
    }
  }
}
```

#### 2. **appsettings.json** (конфигурация)
```json
{
  "AllowedHosts": "*",
  "Urls": "http://0.0.0.0:5008"
}
```

#### 3. **Переменные окружения** (продакшен)
```bash
export ASPNETCORE_URLS="http://0.0.0.0:5008"
export ASPNETCORE_ENVIRONMENT="Production"
```

#### 4. **Program.cs** (программная настройка)
```csharp
// В Program.cs можно добавить:
builder.WebHost.UseUrls("http://0.0.0.0:5008");

// Или через конфигурацию:
builder.Configuration.AddInMemoryCollection(new[]
{
    new KeyValuePair<string, string>("Urls", "http://0.0.0.0:5008")
});
```

### Приоритет настроек (от высшего к низшему):
1. **Переменные окружения** (`ASPNETCORE_URLS`)
2. **Аргументы командной строки** (`--urls`)
3. **launchSettings.json** (только в Development)
4. **appsettings.json**
5. **Значения по умолчанию** (http://localhost:5000)

### 🚀 Практические примеры запуска

#### Запуск на разных портах:
```bash
# Порт 5009
dotnet run --project UsersApp.Web --urls "http://localhost:5009"

# Порт 8080
dotnet run --project UsersApp.Web --urls "http://localhost:8080"

# Несколько портов одновременно
dotnet run --project UsersApp.Web --urls "http://localhost:5008;https://localhost:7118"
```

#### Запуск для внешнего доступа:
```bash
# Доступно извне (0.0.0.0 означает все интерфейсы)
dotnet run --project UsersApp.Web --urls "http://0.0.0.0:5008"

# Доступно только локально
dotnet run --project UsersApp.Web --urls "http://127.0.0.1:5008"
```

#### Запуск в продакшене:
```bash
# Через переменные окружения
export ASPNETCORE_URLS="http://0.0.0.0:80"
export ASPNETCORE_ENVIRONMENT="Production"
dotnet run --project UsersApp.Web

# Или в одной команде
ASPNETCORE_URLS="http://0.0.0.0:80" ASPNETCORE_ENVIRONMENT="Production" dotnet run --project UsersApp.Web
```

#### Проверка текущих настроек:
```bash
# Проверить, какие порты слушает приложение
netstat -tlnp | grep dotnet

# Проверить конкретный порт
lsof -i :5008

# Проверить все порты .NET
ss -tlnp | grep dotnet
```

## 🏗️ Архитектура

Приложение построено по принципам **Clean Architecture** и **SOLID**:

### Слои приложения
- **UsersApp.Domain** - доменный слой (сущности, интерфейсы репозиториев)
- **UsersApp.Application** - слой приложения (бизнес-логика, DTO, сервисы)
- **UsersApp.Infrastructure** - слой инфраструктуры (EF Core, репозитории, SQLite)
- **UsersApp.Web** - веб-слой (MVC контроллеры, API, DI, конфигурация)
- **UsersApp.Tests** - слой тестирования (unit-тесты)

### Функциональность
- ✅ CRUD операции с пользователями
- ✅ Валидация данных на сервере
- ✅ Поиск по имени и email
- ✅ RESTful API
- ✅ Swagger документация
- ✅ Unit-тесты

### Технологии
- **.NET 8.0** - основная платформа
- **ASP.NET Core MVC** - веб-фреймворк
- **Entity Framework Core** - ORM
- **SQLite** - база данных
- **Swagger** - документация API
- **xUnit** - тестирование

## 🧪 Тестирование

```bash
# Запуск unit-тестов (из корневой папки)
dotnet test UsersApp.Tests/UsersApp.Tests.csproj

# Или запуск всех тестов в решении
dotnet test

# Запуск с подробным выводом
dotnet test UsersApp.Tests/UsersApp.Tests.csproj --verbosity normal

# Запуск с покрытием кода
dotnet test UsersApp.Tests/UsersApp.Tests.csproj --collect:"XPlat Code Coverage"
```

### Результаты тестирования
- ✅ **2 теста пройдено** - все unit-тесты работают корректно
- ✅ **Покрытие кода** - тесты покрывают основную бизнес-логику
- ✅ **Быстрое выполнение** - тесты выполняются за ~10ms

## 🚀 Текущий статус приложения

### ✅ Готово к использованию
- **🌐 Приложение запущено**: http://localhost:5008
- **📚 Swagger API**: http://localhost:5008/swagger
- **🧪 Тесты пройдены**: 2/2 тестов успешно
- **🗄️ База данных**: SQLite работает корректно
- **📊 Логи**: Показывают активность приложения

### 🔧 Диагностика
```bash
# Проверить статус приложения
lsof -i :5008

# Проверить логи приложения
# (логи выводятся в консоль при запуске)

# Проверить базу данных
ls -la UsersApp.Web/users.db*
```

## 📁 Структура проекта

```
UsersApp/
├── UsersApp.Domain/          # Доменный слой
│   ├── Entities/            # Сущности (User)
│   └── Repositories/         # Интерфейсы репозиториев
├── UsersApp.Application/     # Слой приложения  
│   ├── Users/               # Сервисы и DTO
│   └── Dtos/                # Data Transfer Objects
├── UsersApp.Infrastructure/ # Слой инфраструктуры
│   ├── Data/                # DbContext
│   └── Repositories/        # Реализации репозиториев
├── UsersApp.Web/            # Веб-слой
│   ├── Controllers/          # MVC и API контроллеры
│   └── Views/               # Razor представления
└── UsersApp.Tests/          # Тесты
    └── UserServiceTests.cs  # Unit-тесты
```