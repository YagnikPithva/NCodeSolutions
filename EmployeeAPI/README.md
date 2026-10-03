# EmployeeAPI

ASP.NET Core 8 Web API for multi-tenant employee management with JWT auth (Admin / Organization / Employee roles).

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server or **SQL Server LocalDB** (default connection uses LocalDB)
- (Optional) [EF Core tools](https://learn.microsoft.com/ef/core/cli/dotnet) for migrations:

```bash
dotnet tool install --global dotnet-ef
```

Update later with:

```bash
dotnet tool update --global dotnet-ef
```

## Configuration

Connection string and JWT settings live in `appsettings.json`:

| Setting | Purpose |
|--------|---------|
| `ConnectionStrings:Default` | SQL Server connection (default: LocalDB → `EmployeeDb`) |
| `Jwt:Key` | Signing key (change in production; min ~32 chars) |
| `Jwt:Issuer` / `Jwt:Audience` | Token issuer/audience |
| `Jwt:ExpiryMinutes` | Access token lifetime |

Override for local work in `appsettings.Development.json` or via environment variables, for example:

```bash
# PowerShell
$env:ConnectionStrings__Default = "Server=.;Database=EmployeeDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

## Setup

From the `EmployeeAPI` folder:

```bash
# Restore packages
dotnet restore

# Build
dotnet build

# Run (HTTP — Swagger at /swagger)
dotnet run --launch-profile http
```

Other profiles (see `Properties/launchSettings.json`):

| Profile | URL(s) |
|---------|--------|
| `http` | http://localhost:5260 |
| `https` | https://localhost:7291 and http://localhost:5260 |
| `IIS Express` | IIS Express ports from launch settings |

```bash
dotnet run --launch-profile https
```

On first run the app:

1. Ensures the database exists (`EnsureCreated`)
2. Seeds a default admin if no users exist:
   - **Username:** `admin`
   - **Password:** `Admin@123`

Swagger UI (Development): http://localhost:5260/swagger

CORS allows common local frontends (`localhost:3000`, `5173`, `5174`, `3001`, `8080`, `4000`).

## Useful scripts

Run these from the project directory (`EmployeeAPI/`):

```bash
# Restore NuGet packages
dotnet restore

# Build (Debug)
dotnet build

# Build (Release)
dotnet build -c Release

# Run
dotnet run --launch-profile http

# Watch mode (reload on file changes)
dotnet watch run --launch-profile http

# Run tests (when a test project is added)
dotnet test

# Publish
dotnet publish -c Release -o ./publish
```

### Health check

```bash
curl http://localhost:5260/api/health
```

### Login (JWT)

```bash
curl -X POST http://localhost:5260/api/auth/login ^
  -H "Content-Type: application/json" ^
  -d "{\"username\":\"admin\",\"password\":\"Admin@123\"}"
```

Paste the returned token into Swagger’s **Authorize** dialog (token only; Swagger adds `Bearer `).

## Database migrations

This project uses **Entity Framework Core** with SQL Server. Migration files live under `Migrations/`.

> **Note:** Startup currently calls `Database.EnsureCreated()`, which creates the schema from the model and **does not apply** EF migrations. Prefer migrations for evolving schemas (see below). If you switch fully to migrations, remove `EnsureCreated` / duplicate ensure calls so EF owns schema changes.

### Install / verify EF tools

```bash
dotnet ef --version
```

### Create a new migration

After changing entities or `AppDbContext`:

```bash
dotnet ef migrations add <MigrationName>
```

Example:

```bash
dotnet ef migrations add AddEmployeePhoneIndex
```

### Apply migrations to the database

```bash
dotnet ef database update
```

Apply up to a specific migration:

```bash
dotnet ef database update <MigrationName>
```

### Remove the last unapplied migration

```bash
dotnet ef migrations remove
```

### Generate SQL script (no direct DB apply)

Useful for review or DBA deployment:

```bash
# Full script from empty DB
dotnet ef migrations script -o migration.sql

# Incremental script between two migrations
dotnet ef migrations script <FromMigration> <ToMigration> -o migration.sql
```

### Drop the database (destructive)

```bash
dotnet ef database drop --force
```

### Existing migration

| Migration | Description |
|-----------|-------------|
| `20261003104929_InitialCreate` | Creates `Organizations`, `Employees`, and `Users` tables with indexes and FKs |

Recommended local flow when using migrations:

```bash
dotnet ef database update
dotnet run --launch-profile http
```

If the DB was previously created only via `EnsureCreated`, either drop and recreate with `dotnet ef database update`, or align the existing DB with migrations carefully (EF history table `__EFMigrationsHistory`).

## Project layout (high level)

```
EmployeeAPI/
├── Controllers/     # Auth, Admin, OrgEmployees, Me, Health
├── Data/            # AppDbContext
├── DTOs/
├── Migrations/      # EF Core migrations
├── Models/          # Employee, Organization, User, Roles
├── Services/        # Auth, Admin, Employee, Token
├── appsettings.json
└── Program.cs
```

## Roles

| Role | Purpose |
|------|---------|
| `Admin` | Platform admin (seeded user) |
| `Organization` | Org-scoped management |
| `Employee` | Employee self-service (`/api/me`) |
