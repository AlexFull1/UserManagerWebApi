## Description
A concise ASP.NET Core Web API targeting .NET 10 that exposes basic CRUD operations for a `User` resource. The project uses Entity Framework Core with a SQL Server provider by default, has request logging middleware, and includes Swagger for API exploration in Development.

## Technologies
- .NET 10 (ASP.NET Core)
- C#
- Entity Framework Core (SQL Server provider configured in `Program.cs`)
- Swashbuckle/Swagger (OpenAPI)
- dotnet CLI / `dotnet-ef`
- Kestrel (default hosting)

## Project-specific notes / things you must fill
- Repository URL: [TODO: insert repository URL]
- License: [TODO: choose and add license]
- Maintainer / Contact: [TODO: add name/email or link]
- If you do not use SQL Server, update the DB provider and `ConnectionStrings:DefaultConnection` in `appsettings.*.json`.

## Quick overview of implemented code (relevant files)
- `Program.cs` — configures DI, EF Core (`ApplicationContext`), middleware and Swagger.
- `ApplicationContext.cs` — EF Core DbContext with `DbSet<User> Users`.
- `Controllers/UsersController.cs` — Controller exposing user endpoints at `/Users`.
- `Repositories/UserRepository.cs` — CRUD operations against DbContext.
- `Services/UserServices.cs` + `Services/IUserServices.cs` — Business logic and DTO mapping.
- `RequestLoggingMiddleware.cs` — Logs incoming requests and response status codes.
- `Models/User.cs` and DTOs under `DTOs/UsersDTO/` (`AddUserDTO`, `GetUserDTO`, `UpdateUserDTO`).

## Prerequisites
- .NET 10 SDK: https://dotnet.microsoft.com/
- SQL Server (local / container / cloud) OR other EF Core provider — update code if switching providers.
- Optional: `dotnet-ef` CLI tool (install globally if needed):
  - `dotnet tool install --global dotnet-ef`

## Configuration
- Primary connection string key: `ConnectionStrings:DefaultConnection` (read in `Program.cs`).
- Example `appsettings.Development.json` snippet (replace values):
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=WebAPIProjectDb;Trusted_Connection=True;"
  }
}
- Alternatively set via environment variable:
  - `ConnectionStrings__DefaultConnection="Server=...;Database=...;User Id=...;Password=...;"`

## How to run (CLI)
1. Clone repository and change directory:
   - `git clone <repository-url>` [TODO: replace]
   - `cd WebAPIProject`

2. Restore and build:
   - `dotnet restore`
   - `dotnet build`

3. Create & apply EF Core migrations (example using SQL Server):
   - If you need to create an initial migration:
     - `dotnet ef migrations add InitialCreate --project WebAPIProject --startup-project WebAPIProject`
     - [If you run migrations from Visual Studio, use __Package Manager Console__ with `Add-Migration` / `Update-Database`.]
   - Apply migrations:
     - `dotnet ef database update --project WebAPIProject --startup-project WebAPIProject`

   Note: If your startup project differs, set `--startup-project` appropriately.

4. Run the API:
   - `dotnet run --project WebAPIProject`
   - Default listen URLs: `https://localhost:5001` and `http://localhost:5000` (Kestrel defaults). Use the console output to confirm URLs.

5. Open Swagger (in Development mode):
   - `https://localhost:5001/swagger`

## API Endpoints (implemented)
Base route for controller: `/Users` (see `Controllers/UsersController.cs` which is `[Route("[controller]")]`)

- GET /Users
  - Description: Return list of users
  - Response: `200 OK` with JSON array of `GetUserDTO`
  - Example:
    curl -s https://localhost:5001/Users

- GET /Users/{id}
  - Description: Return a single user by id
  - Success: `200 OK` with `GetUserDTO`
  - Not found: `404 Not Found`
  - Example:
    curl -v https://localhost:5001/Users/1

- POST /Users
  - Description: Create a user
  - Request body (JSON) — `AddUserDTO`:
    {
      "name": "Alice",
      "age": 30,
      "balance": 100.5
    }
  - Success: `201 Created` with Location header pointing to `/Users/{id}` and body `GetUserDTO`
  - Example:
    curl -X POST https://localhost:5001/Users \
      -H "Content-Type: application/json" \
      -d '{"name":"Alice","age":30,"balance":100.5}'

- PUT /Users/{id}
  - Description: Update user fields (uses `UpdateUserDTO`)
  - Request body (JSON) — `UpdateUserDTO`:
    {
      "name": "Alice Updated",
      "age": 31
    }
  - Success: `204 No Content`
  - Example:
    curl -X PUT https://localhost:5001/Users/1 \
      -H "Content-Type: application/json" \
      -d '{"name":"Alice Updated","age":31}'

- DELETE /Users/{id}
  - Description: Delete a user by id
  - Success: `204 No Content`
  - Example:
    curl -X DELETE https://localhost:5001/Users/1

DTO shapes (from `DTOs/UsersDTO`):
- `AddUserDTO`: { name: string?, age: int, balance: double? }
- `GetUserDTO`: { id: int, name: string?, age: int, balance: double? }
- `UpdateUserDTO`: { name: string?, age: int }

Model:
- `User` entity (in `Models/User.cs`): `{ Id: int, Name: string?, Age: int, Balance: double? }`

## Database setup & seeding
- The project uses `ApplicationContext` DbContext with `DbSet<User> Users`.
- To create DB schema: use EF Core migrations (see commands above).
- Seeding: No automatic seeder included. To add initial data:
  - Implement seeding in `ApplicationContext` or add a dedicated seeder and call it from `Program.cs` after `app.Build()` and before `app.Run()` (typically gated by `if (app.Environment.IsDevelopment())`).

## Logging & middleware
- `RequestLoggingMiddleware` logs incoming request method+path and the outgoing status code. Registered in `Program.cs` with `app.UseMiddleware<RequestLoggingMiddleware>();`.

## Troubleshooting
- "Connection string not found": confirm `ConnectionStrings:DefaultConnection` exists in active configuration and the correct environment (`DOTNET_ENVIRONMENT`) is selected.
- EF migrations failing: ensure `dotnet-ef` version matches SDK and that `--project`/`--startup-project` point to the correct projects.
- Swagger not visible: ensure `ASPNETCORE_ENVIRONMENT=Development` or remove the environment guard in `Program.cs` for local testing.

## Contributing
- Fork, create a feature branch, add tests where applicable, and open a pull request.
- Follow project conventions and `.editorconfig` if present.

## 📄 License

This project is licensed under the MIT License.
See the LICENSE file for details.

## 👤 Maintainer

Oleksandr Prystenskyi  
GitHub: https://github.com/AlexFull1  

Feel free to open an issue or submit a pull request.
