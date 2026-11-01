# HTNL Shop

[![CI](https://github.com/Rayxez/WebBanLinhKienMayTinh/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/Rayxez/WebBanLinhKienMayTinh/actions/workflows/ci.yml)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4)](https://learn.microsoft.com/aspnet/core/)

HTNL Shop is a full-stack e-commerce application for computers and PC components. Built with ASP.NET Core MVC, Entity Framework Core, ASP.NET Core Identity, and SQL Server, it includes a customer storefront, inventory-aware shopping and checkout, and a role-protected admin area.

> **Portfolio project:** developed from a university project into a production-style application with automated tests, database migrations, containerized setup, and GitHub Actions CI.

## Screenshots

| Storefront | Product catalog |
|---|---|
| ![HTNL Shop storefront](docs/screenshots/01-trang-chu.png) | ![Product catalog](docs/screenshots/02-danh-sach-san-pham.png) |

| Product details | Admin dashboard |
|---|---|
| ![Product details](docs/screenshots/03-chi-tiet-san-pham.png) | ![Admin dashboard](docs/screenshots/05-admin-dashboard.png) |

See the [system architecture and order flow](docs/architecture.md) for more detail.

## Features

### Customer storefront

- Browse, search, sort, filter, and paginate a seeded catalog of 480 products across 16 categories.
- Add products to a session-based guest cart; merge the cart into the customer account after sign-in.
- Check stock during cart updates and checkout; create orders within a database transaction.
- View order history, order details, and account profile.
- Submit product reviews after purchasing the product.

### Admin area

- View sales, pending orders, low-stock products, and best-selling items on the dashboard.
- Create, edit, search, filter, paginate, and remove products; upload product images.
- Manage user accounts and roles.
- Process orders through validated status transitions; cancel orders and restore inventory.

### Security and reliability

- ASP.NET Core Identity password hashing, account lockout, and role-based authorization.
- Antiforgery validation for non-GET requests, including AJAX requests.
- Ownership checks for customer order access.
- Inventory concurrency protection to reduce overselling.
- Security response headers, friendly error pages, and a health endpoint at `/health`.

## Technology

- ASP.NET Core MVC 8 and Razor
- Entity Framework Core 8 and SQL Server
- ASP.NET Core Identity
- xUnit, `WebApplicationFactory`, and SQL Server integration tests
- Docker Compose and GitHub Actions

## Run locally

### Requirements

- .NET 8 SDK
- SQL Server. Windows developers can use SQL Server LocalDB; Docker Compose is available as an alternative.

Clone the repository and run the commands from its root:

```bash
git clone https://github.com/Rayxez/WebBanLinhKienMayTinh.git
cd WebBanLinhKienMayTinh
dotnet restore
dotnet run --project HTNLShop.csproj
```

The default development connection string uses SQL Server LocalDB. In `Development`, the app applies migrations and seeds demo data and accounts. Open the URL printed by `dotnet run`.

Development demo accounts:

| Role | Username | Password |
|---|---|---|
| Admin | `admin` | `Admin@12345` |
| Customer | `customer` | `Customer@123` |

These credentials are for local development only. Do not use them in a deployed environment.

### Run with Docker Compose

Install Docker Desktop or Docker Engine with the Compose plugin. Create a local environment file from the example and replace its placeholder passwords with strong, unique values:

```powershell
Copy-Item .env.example .env
```

```bash
docker compose up --build
```

- Storefront: <http://localhost:8080>
- Health check: <http://localhost:8080/health>
- SQL Server: `localhost:1433`

The `.env` file contains local secrets and must not be committed. Docker Compose applies database migrations and seeds the configured demo accounts.

## Tests

Run the test suite from the repository root:

```bash
dotnet test
```

The suite contains 44 xUnit domain, security, authorization, checkout, and order-lifecycle tests. It uses a separate SQL Server database for each test run. By default, tests use LocalDB on Windows; to use another SQL Server, set `HTNLSHOP_TEST_CONNECTION` before running the tests.

The GitHub Actions workflow runs restore, a Release build, tests with coverage, a vulnerable-package check, and a Docker image build.

## Repository structure

```text
Areas/Admin/    Admin controllers and views
Controllers/    Storefront, cart, account, order, and review endpoints
Data/           EF Core context, entities, seeder, and migrations
Services/       Reusable application services
ViewModels/     View-specific models
Views/          Razor pages and shared layouts
tests/          Automated test suite
docs/           Architecture, screenshots, and project notes
```

## Current scope

- Checkout creates an order; no payment gateway is integrated yet.
- The contact form opens a prefilled email draft in the visitor's email application; the app does not send email from a server.
- Payment sandbox integration, transactional email, deployment, and load testing remain future work.

## Credits

The storefront design builds on Bootstrap-based templates, including HTML Codex and Dashmin. Product images are included for educational and portfolio demonstration purposes.
