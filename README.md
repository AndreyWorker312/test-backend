# UsersApp

Многослойное ASP.NET Core MVC приложение для управления пользователями с соблюдением принципов SOLID.

## 🚀 Быстрый запуск

### Предварительные требования
- **.NET 8.0 SDK** ([скачать](https://dotnet.microsoft.com/download))
- **SQLite** (встроен в .NET)
- **Git** (для клонирования репозитория)



#### 1. Восстановление зависимостей
```bash
# Восстановление всех пакетов NuGet
dotnet restore

# Или для конкретного проекта
dotnet restore UsersApp.Web/UsersApp.Web.csproj
```

#### 2. Создание/обновление базы данных
```bash
# Создание миграций (если нужно)
dotnet ef migrations add InitialCreate --project UsersApp.Infrastructure --startup-project UsersApp.Web

# Применение миграций к базе данных
dotnet ef database update --project UsersApp.Infrastructure --startup-project UsersApp.Web
```

#### 3. Запуск приложения

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

## 📚 API Endpoints

Приложение предоставляет RESTful API для управления пользователями:

### 🔗 Базовый URL
```
http://localhost:5008/api/UsersApi
```

### 📋 Доступные эндпоинты

#### 1. **GET /api/UsersApi** - Получить всех пользователей
```http
GET /api/UsersApi
GET /api/UsersApi?query=search_term
```

**Параметры:**
- `query` (optional) - поисковый запрос по имени или email

**Ответ:**
```json
[
  {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "fullName": "Иван Иванов",
    "email": "ivan@example.com",
    "phone": "+1234567890",
    "address": "Москва, ул. Примерная, 1"
  }
]
```

#### 2. **GET /api/UsersApi/{id}** - Получить пользователя по ID
```http
GET /api/UsersApi/123e4567-e89b-12d3-a456-426614174000
```

**Параметры:**
- `id` (required) - GUID пользователя

**Ответы:**
- `200 OK` - пользователь найден
- `404 Not Found` - пользователь не найден

#### 3. **POST /api/UsersApi** - Создать нового пользователя
```http
POST /api/UsersApi
Content-Type: application/json

{
  "fullName": "Новый Пользователь",
  "email": "new@example.com",
  "phone": "+9876543210",
  "address": "Новый адрес"
}
```

**Ответы:**
- `201 Created` - пользователь создан
- `400 Bad Request` - ошибка валидации
- `409 Conflict` - email/телефон уже используется

#### 4. **PUT /api/UsersApi/{id}** - Обновить пользователя
```http
PUT /api/UsersApi/123e4567-e89b-12d3-a456-426614174000
Content-Type: application/json

{
  "fullName": "Обновленное Имя",
  "email": "updated@example.com",
  "phone": "+1111111111",
  "address": "Обновленный адрес"
}
```

**Ответы:**
- `200 OK` - пользователь обновлен
- `400 Bad Request` - ошибка валидации
- `404 Not Found` - пользователь не найден
- `409 Conflict` - email/телефон уже используется

#### 5. **DELETE /api/UsersApi/{id}** - Удалить пользователя
```http
DELETE /api/UsersApi/123e4567-e89b-12d3-a456-426614174000
```

**Ответы:**
- `204 No Content` - пользователь удален
- `404 Not Found` - пользователь не найден

### 🔧 Примеры использования

#### Получить всех пользователей:
```bash
curl -X GET "http://localhost:5008/api/UsersApi"
```

#### Поиск пользователей:
```bash
curl -X GET "http://localhost:5008/api/UsersApi?query=Иван"
```

#### Создать пользователя:
```bash
curl -X POST "http://localhost:5008/api/UsersApi" \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "Тест Пользователь",
    "email": "test@example.com",
    "phone": "+1234567890",
    "address": "Тестовый адрес"
  }'
```

#### Обновить пользователя:
```bash
curl -X PUT "http://localhost:5008/api/UsersApi/123e4567-e89b-12d3-a456-426614174000" \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "Обновленное Имя",
    "email": "updated@example.com",
    "phone": "+9876543210",
    "address": "Новый адрес"
  }'
```

#### Удалить пользователя:
```bash
curl -X DELETE "http://localhost:5008/api/UsersApi/123e4567-e89b-12d3-a456-426614174000"
```

### 📖 Swagger документация

Полная интерактивная документация API доступна по адресу:
**http://localhost:5008/swagger**

В Swagger UI вы можете:
- Просмотреть все эндпоинты
- Протестировать API прямо в браузере
- Увидеть схемы запросов и ответов
- Скачать OpenAPI спецификацию



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