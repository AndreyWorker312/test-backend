# UsersApp

Многослойное ASP.NET Core MVC приложение (Domain / Application / Infrastructure / Web) с EF Core + SQLite.

## Запуск
1) dotnet restore
2) dotnet ef database update --project UsersApp.Infrastructure --startup-project UsersApp.Web
3) dotnet run --project UsersApp.Web

API Swagger: /swagger
UI (MVC): /
Поиск: /Users?q=term

## Архитектура
- UsersApp.Domain: сущности и абстракции
- UsersApp.Application: бизнес-логика и DTO
- UsersApp.Infrastructure: EF Core, репозитории
- UsersApp.Web: MVC + API, DI, конфиг
- UsersApp.Tests: unit-тесты сервиса