# TaskManager

[![CI](https://github.com/NataliaMity/bestmaneger/actions/workflows/ci.yml/badge.svg)](https://github.com/NataliaMity/bestmaneger/actions/workflows/ci.yml)

Канбан-API (доски → колонки → задачи) на .NET 10, чистая архитектура.

## Структура

| Проект | Что внутри | Зависит от |
|---|---|---|
| `Domain` | Сущности, бизнес-правила, интерфейсы репозиториев | — |
| `Application` | Use case'ы (`*Handler` + команда/запрос), DTO | Domain |
| `Infrastructure` | EF Core + PostgreSQL, репозитории, UnitOfWork | Domain |
| `Web` | Minimal API эндпоинты, обработка ошибок, DI | Application, Infrastructure |

Правила:

- **Агрегаты.** `Board` — отдельный агрегат. `Column` — корень агрегата «колонка + задачи»: задачи создаются, переставляются, переносятся и удаляются только через методы колонки. Так порядок (`Order`) всегда остаётся 0..N-1.
- **Сохранение.** Репозитории только отслеживают изменения (`Add`/`Remove`), сохраняет хендлер через `IUnitOfWork.SaveChangesAsync`.
- **Ошибки.** Нарушение бизнес-правила — `DomainException` → 400; нет сущности — `NotFoundException` → 404. Ответ в формате ProblemDetails.
- **Наружу — только DTO**, доменные сущности из Application не выходят.
- **Новый хендлер** регистрируется в DI автоматически (любой public-класс `*Handler` в сборке Application).

## Запуск

1. Строка подключения — `Web/appsettings.Development.json` (`ConnectionStrings:DefaultConnection`) или user-secrets.
2. Миграции:
   ```bash
   dotnet tool install --global dotnet-ef
   dotnet ef migrations add Initial --project Infrastructure --startup-project Web --output-dir Data/Migrations
   dotnet ef database update --project Infrastructure --startup-project Web
   ```
3. `dotnet run --project Web`, сценарий запросов — в `Web/Web.http`, OpenAPI — `/openapi/v1.json`.

## API

| Метод | Путь | |
|---|---|---|
| GET | `/api/boards` | список досок |
| POST | `/api/boards` | создать доску `{ "name", "description" }` |
| GET | `/api/boards/get/{id}` | доска целиком (колонки + задачи) |
| PUT | `/api/boards/update/{id}` | изменить (null-поля не меняются) |
| DELETE | `/api/boards/delete/{id}` | удалить вместе с колонками и задачами |
| GET | `/api/columns/{boardId}` | колонки доски |
| POST | `/api/columns` | добавить колонку в конец `{ "boardId", "name" }` |
| PUT | `/api/columns/update/{id}` | переименовать |
| DELETE | `/api/columns/delete/{id}` | удалить |
| PUT | `/api/columns/reorder/{id}` | `{ "index": 0 }` |
| GET | `/api/tasks/{columnId}` | задачи колонки |
| GET | `/api/tasks/get/{id}` | задача |
| POST | `/api/tasks` | добавить задачу в конец `{ "columnId", "name", "description", "deadline" }` |
| PUT | `/api/tasks/update/{id}` | изменить (`"removeDeadline": true` — снять дедлайн) |
| DELETE | `/api/tasks/delete/{id}` | удалить |
| PUT | `/api/tasks/reorder/{id}` | `{ "index": 0 }` — внутри колонки |
| PUT | `/api/tasks/move/{id}` | `{ "columnId": "...", "index": 0 }` — в другую колонку (без `index` — в конец) |

## Тесты

```bash
dotnet test
```

`Domain.UnitTests` — логика агрегатов, `Application.UnitTests` — хендлеры на in-memory фейках репозиториев.
