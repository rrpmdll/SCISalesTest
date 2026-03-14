# SCISalesTest - Product Management System

Technical assessment for Senior Developer — Product Management System built with **ASP.NET Core 8**, **SQL Server Stored Procedures**, **Dapper**, and **Clean Architecture**.

---

## Table of Contents

- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Architecture Overview](#architecture-overview)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [Running with Docker](#running-with-docker)
- [API Documentation](#api-documentation)
- [Running Tests](#running-tests)
- [External API Integration](#external-api-integration)
- [Design Patterns & Principles](#design-patterns--principles)

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB, Express, or Developer Edition)
- [Docker](https://www.docker.com/products/docker-desktop/) (optional, for containerized execution)

---

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/rrpmdll/SCISalesTest.git
cd SCISalesTest
```

### 2. Set Up the Database

Open SSMS, connect to your SQL Server instance, and execute the database script:

```
scripts/database.sql
```

Or via command line:

```bash
sqlcmd -S localhost -i scripts/database.sql
```

> If you use SQL Server Express (named instance), use: `sqlcmd -S localhost\SQLEXPRESS -E -i scripts/database.sql`

This will:
- Create the `SCISalesTestDb` database
- Create the `Products` table
- Create all 5 stored procedures (`SP_CreateProduct`, `SP_GetAllProducts`, `SP_GetProductById`, `SP_UpdateProduct`, `SP_DeleteProduct`)
- Insert 5 sample products as seed data

### 3. Verify the Connection String

Ensure `webapi/WebApi/appsettings.Development.json` has the correct server instance:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SCISalesTestDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> **Note:** If you use SQL Server Express, change `localhost` to `localhost\SQLEXPRESS`.

### 4. Start the WebApi (Backend)

Open a terminal and run:

```bash
cd webapi/WebApi
dotnet run --launch-profile https
```

The API will start on:
- **https://localhost:5000** (HTTPS)
- **http://localhost:5001** (HTTP)
- Swagger UI: **https://localhost:5000/swagger**
- Health Check: **https://localhost:5000/health**

Wait until you see `Now listening on...` before proceeding.

### 5. Start the WebApp (Frontend)

Open a **second terminal** and run:

```bash
cd webapp/WebApp
dotnet run --launch-profile https
```

The MVC app will start on:
- **https://localhost:5002** (HTTPS)
- **http://localhost:5003** (HTTP)

> The WebApp connects to the WebApi at `http://localhost:5001` (configured in `webapp/WebApp/appsettings.json` under `WebApi.BaseAddress`).

### 6. Verify Everything Works

1. Open **https://localhost:5002** in a browser
2. Navigate to **Products** — you should see the 5 seeded products
3. Test CRUD operations (Create, Edit, Delete)
4. Test the **Currency Converter** page

### 7. Run the Tests

```bash
dotnet test SCISalesTest.sln
```

Expected result: **19 tests passed** (13 unit + 6 integration).

---

## Architecture Overview

This solution follows a **hybrid Clean Architecture** approach:

- **WebApi** (Backend): Clean Architecture with CQRS pattern
  - `Domain` — Entities, Repositories (interfaces), Constants, Enums, Exceptions, Options
  - `Application` — CQRS Commands/Queries (MediatR), DTOs, Application Services, AutoMapper Profiles, External Service Interfaces
  - `Infrastructure` — Dapper Context, Stored Procedure Helpers, Repository Implementations, External Service Implementations, DI Configuration
  - `WebApi` — REST Controllers, Swagger, Health Checks, Global Exception Filter

- **WebApp** (Frontend): ASP.NET Core MVC consuming the WebApi via HttpClient
  - Razor Views with Bootstrap 5
  - Typed HttpClient service for API communication
  - Client & server-side validation

- **Tests**: Unit Tests (xUnit + Moq) and Integration Tests (WebApplicationFactory)

```
┌──────────────────────────────────────────────────────────────┐
│                     WebApp (MVC - Razor)                      │
│         Bootstrap 5 · HttpClient → WebApi                     │
└──────────────────────┬───────────────────────────────────────┘
                       │ HTTP
┌──────────────────────▼───────────────────────────────────────┐
│                   WebApi (REST Controllers)                    │
│          Swagger · Health Checks · Exception Filter           │
├───────────────────────────────────────────────────────────────┤
│                   Application (CQRS + Services)               │
│    MediatR Handlers · AutoMapper · DTOs · ProductService      │
├───────────────────────────────────────────────────────────────┤
│                   Domain (Core Business Logic)                │
│      Entities · Repositories · Constants · Exceptions         │
├───────────────────────────────────────────────────────────────┤
│                 Infrastructure (Data Access)                   │
│     Dapper · Stored Procedures · ExchangeRate HTTP Client     │
└───────────────────────┬───────────────────────────────────────┘
                        │
                   SQL Server
```

---

## Technology Stack

| Component              | Technology                          |
|------------------------|-------------------------------------|
| Framework              | .NET 8 (ASP.NET Core)               |
| Backend API            | ASP.NET Core Web API                |
| Frontend               | ASP.NET Core MVC (Razor + Bootstrap 5) |
| Database               | SQL Server                          |
| ORM / Data Access      | Dapper + Stored Procedures          |
| CQRS                   | MediatR 12.x                        |
| Object Mapping         | AutoMapper 13.x                     |
| API Documentation      | Swagger (Swashbuckle)               |
| Telemetry              | Application Insights                |
| Health Checks          | ASP.NET Core Health Checks + SqlServer check |
| Unit Tests             | xUnit + Moq                         |
| Integration Tests      | WebApplicationFactory + xUnit       |
| Containerization       | Docker + Docker Compose             |

---

## Project Structure

```
SCISalesTest/
├── webapi/                        # Backend (Clean Architecture)
│   ├── Domain/                    # Core domain layer
│   │   ├── Constants/             # Application constants
│   │   ├── Entities/              # Domain entities (Product, BaseEntity)
│   │   ├── Enums/                 # Enumerations
│   │   ├── Exceptions/            # Custom exceptions
│   │   ├── Extensions/            # Extension methods
│   │   ├── Options/               # Configuration options (IOptions pattern)
│   │   └── Repositories/          # Repository interfaces
│   ├── Application/               # Application services layer
│   │   ├── ApplicationServices/   # Business logic services
│   │   ├── DTOs/                  # Data transfer objects
│   │   ├── ExternalServices/      # External service interfaces
│   │   └── Feature/               # CQRS Commands, Queries, Handlers, Profiles
│   ├── Infrastructure/            # Infrastructure layer
│   │   ├── Context/               # DapperContext (DB connection)
│   │   ├── Extensions/            # DI registration extensions
│   │   ├── Helpers/               # StoredProcedureHelper
│   │   ├── Repositories/          # Repository & UnitOfWork implementations
│   │   └── Services/              # External service implementations
│   └── WebApi/                    # API entry point
│       ├── Base/Filters/          # Global exception filter
│       ├── Controllers/           # REST API controllers
│       ├── Properties/            # Launch settings
│       └── Dockerfile
├── webapp/                        # Frontend (MVC)
│   └── WebApp/
│       ├── Controllers/           # MVC controllers
│       ├── Models/                # View models
│       ├── Services/              # API client services
│       ├── Views/                 # Razor views
│       ├── wwwroot/               # Static files
│       └── Dockerfile
├── tests/                         # Test projects
│   ├── Application.UnitTest/      # Unit tests (Services & Handlers)
│   └── WebApi.IntegrationTest/    # Integration tests (API endpoints)
├── scripts/
│   └── database.sql               # SQL script (table + stored procedures + seed data)
├── docker-compose.yml
├── SCISalesTest.sln
└── README.md
```

---

## Running with Docker

```bash
docker-compose up --build
```

This starts:
- **SQL Server** on port 1433
- **WebApi** on port 5000
- **WebApp** on port 5002

> **Note:** After starting with Docker, execute the database script against the containerized SQL Server:
> ```bash
> docker exec -i scisalestest-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStr0ngP@ssword!" -C -i /dev/stdin < scripts/database.sql
> ```

---

## API Documentation

Once the WebApi is running, Swagger UI is available at `/swagger`.

### Endpoints

| Method | Endpoint                                   | Description                          |
|--------|--------------------------------------------|--------------------------------------|
| GET    | `/api/product`                             | Get all active products              |
| GET    | `/api/product/{id}`                        | Get product by ID                    |
| POST   | `/api/product`                             | Create a new product                 |
| PUT    | `/api/product/{id}`                        | Update an existing product           |
| DELETE | `/api/product/{id}`                        | Soft-delete a product                |
| GET    | `/api/product/{id}/exchange-rate?targetCurrency=COP` | Get product with converted price |
| GET    | `/health`                                  | Health check endpoint                |

---

## Running Tests

### All Tests
```bash
dotnet test SCISalesTest.sln
```

### Unit Tests Only
```bash
dotnet test tests/Application.UnitTest/Application.UnitTest.csproj
```

### Integration Tests Only
```bash
dotnet test tests/WebApi.IntegrationTest/WebApi.IntegrationTest.csproj
```

---

## External API Integration

The application integrates with the **Open Exchange Rates API** ([open.er-api.com](https://open.er-api.com)) to provide live currency conversion for product prices.

### Use Case
- From the product list, use the built-in currency converter or navigate to the standalone Currency Converter page
- Select source/target currencies and enter an amount
- The system fetches live exchange rates and displays the converted price

### API Endpoint
```
GET /api/product/{id}/exchange-rate?targetCurrency=COP
```

### Example Response
```json
{
  "id": 1,
  "name": "Wireless Keyboard",
  "description": "Ergonomic wireless keyboard with Bluetooth connectivity",
  "originalPrice": 49.99,
  "originalCurrency": "USD",
  "convertedPrice": 209958.00,
  "targetCurrency": "COP",
  "exchangeRate": 4200.00,
  "createdDate": "2026-03-13T00:00:00"
}
```

---

## Design Patterns & Principles

- **Clean Architecture** — Domain at the center, no outward dependencies
- **CQRS** — Command/Query separation via MediatR
- **Repository Pattern** — Abstracted data access through interfaces
- **Unit of Work** — Transaction management
- **DTO Pattern** — Data transfer objects for API boundaries
- **Options Pattern** — Strongly-typed configuration via `IOptions<T>`
- **Global Exception Handling** — Centralized error handling with custom exceptions
- **Dependency Injection** — Configured in Infrastructure extensions


