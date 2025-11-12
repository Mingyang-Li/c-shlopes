# ⛷️ ❄️ Ski Field Tracker

Ski Field Tracker is a .NET 8 Web API for managing ski resort information. The repo is structured as a future C# + React monorepo; for now only the backend lives under `backend/SkiFieldTracker.Api`.

## Quick Start

```bash
# move into the repo root
cd /Users/mingyangli/Desktop/dev-personal/c-shlopes

# 1. Boot PostgreSQL via the MSBuild helper target (wraps docker compose)
dotnet msbuild backend/SkiFieldTracker.Api/SkiFieldTracker.Api.csproj -t:DockerPostgresUp

# 2. Apply migrations and seed the database (idempotent)
dotnet run --project backend/SkiFieldTracker.Api -- --seed

# 3. Run the API
dotnet run --project backend/SkiFieldTracker.Api
```

To stop the local database:

```bash
dotnet msbuild backend/SkiFieldTracker.Api/SkiFieldTracker.Api.csproj -t:DockerPostgresDown
```

Default connection string:
`Host=localhost;Port=5432;Database=ski_field_tracker;Username=ski_admin;Password=ski_admin_pw`

Swagger UI is available at:
- https://localhost:7127/swagger
- http://localhost:5127/swagger

## Tech Stack & Layout

- ASP.NET Core Web API (single deployable project)
- Entity Framework Core + Npgsql with migrations for schema definition
- Clean architecture-style folders inside one project: `Domain`, `Application`, `Infrastructure`, `Controllers`
- Swagger via Swashbuckle with accurate OpenAPI metadata
- Docker Compose helper to start PostgreSQL
- `Seeds/ski-fields.json` provides 10 sample resorts

## REST Endpoints

| Method & Path               | Description                                       |
| --------------------------- | ------------------------------------------------- |
| `POST /api/skifields`       | Create a ski field (409 on duplicate `name`)      |
| `POST /api/skifields/query` | Prisma `findMany`-style filtering + pagination    |
| `PUT /api/skifields/{id}`   | Update an existing ski field                      |
| `DELETE /api/skifields/{id}`| Delete a ski field                                |

### Prisma-style Querying

`POST /api/skifields/query` accepts a JSON payload (in the body or as a URL-encoded `query` parameter) that mirrors Prisma `findMany` semantics:

```json
{
  "where": {
    "name": { "contains": "vale", "mode": "insensitive" },
    "country": { "equals": "United States" },
    "adultFullDayPassUsd": { "gte": 150, "lte": 220 }
  },
  "orderBy": [
    { "field": "adultFullDayPassUsd", "direction": "asc" }
  ],
  "skip": 0,
  "take": 10
}
```

The response returns `items`, `totalCount`, `skip`, and `take`.

## Migrations & Seeding

- Generated migrations live in `backend/SkiFieldTracker.Api/Migrations`.
- Running the API with `--seed` applies migrations and inserts seed data (name-based dedupe, safe to re-run).
- Normal startup also applies pending migrations and backfills missing seed entries.

## Next Steps

- When the React front end is ready, create a `/frontend` folder at the repo root; the backend is self-contained under `backend/SkiFieldTracker.Api`.
- Add linting/formatting (`.editorconfig`, `dotnet format`, StyleCop) as needed for team standards.
