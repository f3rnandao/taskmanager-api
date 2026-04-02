# TaskManager API

A RESTful API for task management built with .NET 8 and Clean Architecture.

## Tech Stack

- **Backend:** .NET 8, C#, ASP.NET Core
- **Architecture:** Clean Architecture
- **Database:** PostgreSQL *(coming soon)*
- **Authentication:** JWT *(coming soon)*
- **CI/CD:** GitHub Actions *(coming soon)*

## Project Structure
```
taskmanager-api/
├── TaskManager.API/           # Entry point, controllers, middleware
├── TaskManager.Application/   # Use cases, interfaces, DTOs
├── TaskManager.Domain/        # Entities, domain rules
├── TaskManager.Infrastructure/# Database, external services
└── TaskManager.sln
```

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Running locally
```bash
git clone https://github.com/f3rnandao/taskmanager-api.git
cd taskmanager-api
dotnet restore
dotnet run --project TaskManager.API
```

## Architecture

This project follows Clean Architecture principles:

- **Domain** — core entities and business rules, no external dependencies
- **Application** — use cases and interfaces, depends only on Domain
- **Infrastructure** — database and external services, implements Application interfaces
- **API** — entry point, depends on Application and Infrastructure

## Roadmap

- [ ] Task CRUD endpoints
- [ ] JWT authentication
- [ ] PostgreSQL integration
- [ ] Docker support
- [ ] CI/CD pipeline
- [ ] Azure deployment

## License

MIT