# TaskManager API

![Build and Test](https://github.com/f3rnandao/taskmanager-api/actions/workflows/build.yml/badge.svg)

A RESTful API for task and project management built with .NET 8, Clean Architecture, PostgreSQL, Docker, and GitHub Actions CI/CD.

## Tech Stack

- **Runtime**: .NET 8 / C#
- **Architecture**: Clean Architecture with Domain-Driven Design
- **ORM**: Entity Framework Core 8
- **Database**: PostgreSQL 16
- **Containerization**: Docker + docker-compose
- **Testing**: xUnit + Moq + FluentAssertions (10 unit tests)
- **CI/CD**: GitHub Actions
- **Documentation**: Swagger / OpenAPI

## Architecture Overview

Dependencies always point inward — outer layers know about inner layers, never the reverse.
TaskManager.Domain          → Business entities and rules (no external dependencies)
TaskManager.Application     → Use cases, interfaces, DTOs, services
TaskManager.Infrastructure  → EF Core, PostgreSQL, repository implementations
TaskManager.API             → Controllers, dependency injection, middleware
TaskManager.Tests           → Unit tests for Application layer

### Key Design Decisions

- **Private constructors + factory methods** enforce valid entity state at creation time
- **Repository pattern** decouples business logic from data access
- **Interface-based dependencies** allow easy testing without a real database
- **Enums stored as int** in the database for performance
- **UTC timestamps** stored in the database, converted to America/Sao_Paulo in responses

## Domain Model
Project
├── Id (Guid)
├── Name (required, max 200)
├── Description (optional, max 1000)
├── CreatedAt (UTC)
└── Tasks (List<TaskItem>)
TaskItem
├── Id (Guid)
├── Title (required, max 200)
├── Description (optional, max 1000)
├── Status (Todo = 1, InProgress = 2, Done = 3)
├── Priority (Low = 1, Medium = 2, High = 3, Critical = 4)
├── ProjectId (foreign key → Project)
├── CreatedAt (UTC)
└── CompletedAt (UTC, nullable)

## API Endpoints

### Projects

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/projects | List all projects |
| GET | /api/projects/{id} | Get project by id |
| POST | /api/projects | Create a new project |

### Tasks

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/tasks/project/{projectId} | List tasks by project |
| POST | /api/tasks | Create a new task |
| PATCH | /api/tasks/{id}/complete | Mark task as complete |

## Running Locally

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [EF Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet): `dotnet tool install --global dotnet-ef --version 8.0.0`

### Steps

**1. Clone the repository**
```bash
git clone https://github.com/f3rnandao/taskmanager-api.git
cd taskmanager-api
```

**2. Start PostgreSQL**
```bash
docker-compose up -d
```

**3. Apply database migrations**
```bash
dotnet ef database update \
  --project TaskManager.Infrastructure/TaskManager.Infrastructure.csproj \
  --startup-project TaskManager.API/TaskManager.API.csproj
```

**4. Run the API**
```bash
dotnet run --project TaskManager.API/TaskManager.API.csproj
```

**5. Open Swagger UI**
http://localhost:5218/swagger

## Running Tests

```bash
dotnet test --verbosity normal
```

10 unit tests cover ProjectService and TaskService using mocked repositories — no database required.

## Project Structure

taskmanager-api/
├── TaskManager.Domain/
│   ├── Entities/
│   │   ├── Project.cs
│   │   └── TaskItem.cs
│   └── Enums/
│       ├── WorkTaskStatus.cs
│       └── TaskPriority.cs
├── TaskManager.Application/
│   ├── DTOs/
│   ├── Interfaces/
│   └── Services/
├── TaskManager.Infrastructure/
│   ├── Persistence/
│   │   ├── AppDbContext.cs
│   │   └── Configurations/
│   └── Repositories/
├── TaskManager.API/
│   └── Controllers/
├── TaskManager.Tests/
│   └── Services/
├── docker-compose.yml
└── .github/
└── workflows/
└── build.yml

## Roadmap

- [x] Clean Architecture structure
- [x] Domain entities with encapsulation and factory methods
- [x] RESTful API with Swagger documentation
- [x] PostgreSQL with Entity Framework Core
- [x] Docker + docker-compose
- [x] Unit tests with xUnit, Moq and FluentAssertions
- [x] GitHub Actions CI/CD pipeline
- [ ] JWT authentication
- [ ] Azure deployment

## License

MIT