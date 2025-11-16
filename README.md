# ⛷️ ❄️ Ski Field Tracker (SkiRadar24)

A full-stack web application for discovering and managing ski resorts worldwide. Built with .NET 8, Next.js, and deployed on Azure & Vercel.

## 🎥 Demo

** Watch the demo video **
https://www.loom.com/share/01acb0d80a1747178c365e21cbd8a124

## 🚀 Tech Stack

### Backend (API)
- **.NET 8** - Modern C# web framework
- **ASP.NET Core Web API** - RESTful API
- **Entity Framework Core** - ORM with migrations
- **PostgreSQL** - Relational database (Supabase)
- **Npgsql** - PostgreSQL .NET driver
- **Swagger/OpenAPI** - API documentation

### Frontend (UI)
- **Next.js 16** - React framework with App Router
- **TypeScript** - Type-safe development
- **shadcn/ui** - Beautiful, accessible component library
- **Tailwind CSS v4** - Utility-first CSS framework
- **React Query (TanStack Query)** - Data fetching and caching
- **Zod** - Schema validation
- **next-themes** - Dark/light mode support
- **Yarn** - Package manager

### Infrastructure & Deployment
- **Azure App Service** - Backend hosting (Basic B1)
- **Vercel** - Frontend hosting
- **Supabase** - PostgreSQL database hosting
- **GitHub Actions** - CI/CD pipeline
- **Docker** - Local PostgreSQL container

## 📋 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/) and [Yarn](https://yarnpkg.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (for local database)
- [Git](https://git-scm.com/)

## 🛠️ Local Setup

### 1. Clone the Repository

```bash
git clone <repository-url>
cd c-shlopes
```

### 2. Backend Setup

#### Start Local PostgreSQL Database

```bash
# Start PostgreSQL via Docker Compose
dotnet msbuild backend/SkiFieldTracker.Api/SkiFieldTracker.Api.csproj -t:DockerPostgresUp
```

This will start a PostgreSQL container with:
- Host: `localhost`
- Port: `5432`
- Database: `ski_field_tracker`
- Username: `ski_admin`
- Password: `ski_admin_pw`

#### Apply Migrations and Seed Data

```bash
# Apply migrations and seed the database
dotnet run --project backend/SkiFieldTracker.Api -- --seed
```

#### Run the API

```bash
# Start the API server
dotnet run --project backend/SkiFieldTracker.Api
```

The API will be available at:
- **Swagger UI**: https://localhost:7127/swagger
- **HTTP**: http://localhost:5127
- **HTTPS**: https://localhost:7127

#### Stop the Database (when done)

```bash
dotnet msbuild backend/SkiFieldTracker.Api/SkiFieldTracker.Api.csproj -t:DockerPostgresDown
```

### 3. Frontend Setup

#### Install Dependencies

```bash
cd frontend
yarn install
```

#### Configure Environment Variables

Create a `.env.local` file in the `frontend` directory:

```bash
# For local development
NEXT_PUBLIC_API_URL=http://localhost:5127
```

For production, set this to your Azure App Service URL.

#### Run the Development Server

```bash
yarn dev
```

The frontend will be available at:
- **Local**: http://localhost:3000

### 4. Verify Everything Works

1. **Backend**: Visit http://localhost:5127/swagger to see the API documentation
2. **Frontend**: Visit http://localhost:3000 to see the application
3. **Test API**: The frontend should automatically fetch data from the backend

## 📁 Project Structure

```
c-shlopes/
├── backend/
│   └── SkiFieldTracker.Api/          # .NET 8 Web API
│       ├── Application/               # Business logic & use cases
│       ├── Domain/                    # Entities & domain models
│       ├── Infrastructure/             # EF Core, repositories, persistence
│       ├── Controllers/               # API endpoints
│       └── Migrations/                # Database migrations
├── frontend/                          # Next.js application
│   └── src/
│       ├── app/                       # Next.js App Router pages
│       ├── components/                # React components (shadcn/ui)
│       ├── hooks/                     # Custom React hooks
│       ├── lib/                       # Utilities & API client
│       └── types/                     # TypeScript type definitions
└── .github/
    └── workflows/                      # GitHub Actions CI/CD
```

## 🔌 API Endpoints

| Method & Path               | Description                                       |
| --------------------------- | ------------------------------------------------- |
| `POST /api/skifields`       | Create a ski field (409 on duplicate `name`)      |
| `POST /api/skifields/query` | Prisma `findMany`-style filtering + pagination    |
| `PUT /api/skifields/{uid}`  | Update an existing ski field                      |
| `DELETE /api/skifields/{uid}`| Delete a ski field                                |

### Prisma-style Querying

`POST /api/skifields/query` accepts a JSON payload that mirrors Prisma `findMany` semantics:

```json
{
  "where": {
    "name": { "contains": "vale", "mode": "insensitive" },
    "countryCode": { "equals": "USA" },
    "fullDayPassPrice": { "gte": 150, "lte": 220 }
  },
  "orderBy": [
    { "field": "fullDayPassPrice", "direction": "asc" }
  ],
  "skip": 0,
  "take": 10
}
```

The response returns `items` and `totalCount`.

## 🗄️ Database

### Migrations & Seeding

- Migrations live in `backend/SkiFieldTracker.Api/Migrations`
- Running with `--seed` applies migrations and inserts seed data (idempotent)
- Normal startup applies pending migrations automatically

### MSBuild Commands

```bash
# Start database
dotnet msbuild backend/SkiFieldTracker.Api/SkiFieldTracker.Api.csproj -t:DockerPostgresUp

# Stop database
dotnet msbuild backend/SkiFieldTracker.Api/SkiFieldTracker.Api.csproj -t:DockerPostgresDown

# Seed database
dotnet msbuild backend/SkiFieldTracker.Api/SkiFieldTracker.Api.csproj -t:DbSeed

# Reset database (clean + seed)
dotnet msbuild backend/SkiFieldTracker.Api/SkiFieldTracker.Api.csproj -t:DbReset
```

## 🌐 Deployment

### Backend (Azure App Service)

- **URL**: https://skiradar24-gteaeze8hje6gjbn.canadacentral-01.azurewebsites.net
- **Deployment**: Automatic via GitHub Actions on push to `main`
- **Database**: Supabase PostgreSQL
- **Health Check**: `/health` endpoint

See [DEPLOY.md](./DEPLOY.md) for detailed deployment instructions.

### Frontend (Vercel)

- **Production URL**: https://skiradar24.com
- **Deployment**: Automatic via Vercel on push to `main`
- **Environment**: Set `NEXT_PUBLIC_API_URL` to Azure App Service URL

## 🏗️ Architecture

### Backend Architecture

- **Clean Architecture** - Domain, Application, Infrastructure layers
- **Repository Pattern** - Data access abstraction
- **Use Cases** - Business logic encapsulation
- **DTOs** - Data transfer objects for API contracts

### Frontend Architecture

- **App Router** - Next.js 16 App Router with Server/Client Components
- **React Query** - Server state management with caching
- **Component Library** - shadcn/ui for consistent UI
- **Type Safety** - Full TypeScript coverage

## 🔐 Environment Variables

### Backend

Set in Azure App Service Configuration:
- `ConnectionStrings__DefaultConnection` - Supabase PostgreSQL connection string
- `Cors__AllowedOrigins__0` - Frontend URL (e.g., `https://skiradar24.com`)
- `ASPNETCORE_ENVIRONMENT` - `Production`

### Frontend

Set in Vercel Environment Variables:
- `NEXT_PUBLIC_API_URL` - Backend API URL

## 🌟 Vision & Mission

SkiRadar24 is more than just a ski field directory. Our mission is to:

### 🤖 AI-Powered Data Collection
We're building an **AI agent** that will:
- **Daily data aggregation**: Automatically pull ski field information from around the world
- **Price intelligence**: Help skiers discover affordable options and hidden gems
- **Real-time updates**: Keep pricing, conditions, and availability current

### 🏔️ Supporting Local Communities
- **Club fields & community resorts**: Highlight smaller, locally-owned ski areas
- **Community-driven**: Give voice to independent ski fields that might not have big marketing budgets
- **Preservation**: Help preserve and popularize the sport by connecting skiers with diverse options

### 🌍 Bringing the Ski Community Together
- **Global connection**: Connect skiers worldwide with shared passion for the sport
- **Accessibility**: Make skiing more accessible by showcasing budget-friendly options
- **Discovery**: Help skiers explore new destinations beyond the well-known mega-resorts

Our goal is to democratize ski field information, support local economies, and strengthen the global skiing community - one data point at a time.

## 📝 Development Notes

- **Strict TypeScript**: Enabled with `noUncheckedIndexedAccess` and other strict checks
- **Code Style**: Prettier for formatting, ESLint for linting
- **Git Hooks**: Consider adding pre-commit hooks for code quality

## 🚧 Future Enhancements

### Immediate Features
- [ ] Authentication & Authorization (Clerk or Supabase Auth)
- [ ] Country filter dropdown with full country names
- [ ] Advanced filtering (price range, region, etc.)
- [ ] User favorites/bookmarks
- [ ] Search functionality
- [ ] Map integration
- [ ] Weather integration

### AI Agent & Data Pipeline
- [ ] AI agent for automated data collection from ski field websites
- [ ] Daily price monitoring and alerts
- [ ] Web scraping infrastructure for global ski field data
- [ ] Data validation and quality checks
- [ ] Historical price tracking and trends

### Community Features
- [ ] User reviews and ratings
- [ ] Community-driven field submissions
- [ ] Local club field spotlight section
- [ ] Ski field owner dashboard for self-service updates
- [ ] Social features (share trips, connect with other skiers)

## 📄 License

MIT
