# AccountManager – Technical Documentation

## 1. Overview

**AccountManager** is a sample application demonstrating a multi-layered architecture in .NET 9, using:
- **ASP.NET Core Web API** (in **AccountManager.API**)
- **Blazor WebAssembly** client (in **AccountManager.APP**)
- **Entity Framework Core** (in **AccountManager.Repository**)
- **Domain Entities** (in **AccountManager.Domain**)
- **Repository and Service interfaces** (in **AccountManager.Interfaces**)
- **Service implementations** (in **AccountManager.Services**)

It provides a full CRUD solution for managing:
- Accounts and their subscriptions
- Subscription Plans
- Subscription Statuses
- Audit Logs of changes (AccountChangesLog)

The project also includes:
- **Swagger** documentation (via [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore))
- **CORS** configuration
- **Basic Security** considerations (though not a full Identity-based login in this example)
- **Debounce-based searching** in the UI

---

## 2. Solution Structure

The solution **AccountManager** contains six projects:

1. **AccountManager.API**  
   - An ASP.NET Core Web API project targeting `net9.0`.
   - Implements Controllers for Accounts, Subscriptions, Statuses, and Logs.
   - Uses **Dependency Injection** to connect repositories and services.
   - Sets up Swagger, JSON options, and CORS policies.

2. **AccountManager.APP**  
   - A Blazor WebAssembly application targeting `net9.0`.
   - Contains razor pages/components for the UI, including CRUD for Accounts and Subscriptions, and a Logs view.
   - Leverages **HttpClient** to call the `AccountManager.API` endpoints.

3. **AccountManager.Domain**  
   - Defines **entities** (models) for the system:
     - `Account`
     - `AccountChangesLog`
     - `AccountSubscription`
     - `AccountSubscriptionStatus`
     - `Subscription`
   - Includes data annotations (`[Key]`, `[DatabaseGenerated]`, etc.) for Entity Framework Core.

4. **AccountManager.Interfaces**  
   - Houses **Interfaces** for Repositories and Services, laying out the contract for:
     - `IAccountRepository`, `IAccountChangesLogRepository`, `ISubscriptionRepository`, `IAccountSubscriptionStatusRepository`
     - `IAccountService`, `IAccountChangesLogService`, `ISubscriptionService`, `IAccountSubscriptionStatusService`
   - Promotes a **clean separation** of concerns between data-access details and the rest of the system.

5. **AccountManager.Repository**  
   - Implements **Entity Framework Core** data-access logic.
   - Contains the `ApplicationDbContext` (inherits from `IdentityDbContext`).
   - Provides `AccountRepository`, `AccountChangesLogRepository`, `SubscriptionRepository`, and `AccountSubscriptionStatusRepository`.

6. **AccountManager.Services**  
   - Implements the **business logic** layer.
   - Classes like `AccountService`, `AccountChangesLogService`, `SubscriptionService`, `AccountSubscriptionStatusService` encapsulate logic and validations, and orchestrate calls to the repositories.

A typical **flow** for a request looks like:
```
Blazor UI -> [HTTP call] -> AccountManager.API (Controllers) -> [Dependency Injection]
   -> Services (Business Logic) -> Repositories (EF Core) -> Database
```

---

## 3. Technologies and Libraries

1. **.NET 9** – C# 12 features, latest Microsoft.NET.Sdk style projects.
2. **ASP.NET Core Web API** – For endpoints (in `AccountManager.API`).
3. **Blazor WebAssembly** – SPA front-end (in `AccountManager.APP`).
4. **Entity Framework Core 9.0** – Data access (in `AccountManager.Repository`).
5. **Swashbuckle.AspNetCore (8.x)** – Swagger/OpenAPI generation.
6. **Microsoft.AspNetCore.Components.WebAssembly** – Blazor WebAssembly hosting model.
7. **System.Text.Json** with `ReferenceHandler.IgnoreCycles` – JSON serialization settings.
8. **Microsoft.AspNetCore.Identity.EntityFrameworkCore** – For `IdentityDbContext` inheritance in `ApplicationDbContext`.

---

## 4. Database Schema and Entities

### 4.1 Database Schema

All entities are mapped via **Entity Framework Core** in `ApplicationDbContext`. Notable `DbSet`s:

- `DbSet<Account> Accounts`
- `DbSet<AccountSubscription> AccountSubscriptions`
- `DbSet<Subscription> Subscriptions`
- `DbSet<AccountSubscriptionStatus> AccountSubscriptionStatuses`
- `DbSet<AccountChangesLog> AccountChangesLogs`

### 4.2 Entities

1. **Account**
   - **Primary Key**: `AccountId` (int, identity)
   - **Fields**: `CompanyName`, `IsActive`, `Country`, `CreatedAt`, `Token`, `Is2FAEnabled`, `IsIPFilterEnabled`, `IsSessionTimeoutEnabled`, `SessionTimeOut`, etc.
   - **Navigation**: `AccountSubscription` (1–1 with `AccountSubscription`)

2. **AccountSubscription**
   - **Primary Key**: `AccountSubscriptionId`
   - **Foreign Keys**: `SubscriptionId` → `Subscription`; `AccountId` → `Account`; `SubscriptionStatusId` → `AccountSubscriptionStatus`
   - **Fields**: `Is2FaAllowed`, `IsIpFilterAllowed`, `IsSessionTimeoutAllowed`
   - This table holds the *current subscription plan* and *status* references for a single account.

3. **Subscription**
   - **Primary Key**: `SubscriptionId`
   - **Fields**: `Description`, `IsDefault`, `IsActive`, `AvailableYearly`, `Is2FaAllowed`, `IsIpFilterAllowed`, `IsSessionTimeoutAllowed`
   - Acts as a *subscription plan template* that an account can adopt.

4. **AccountSubscriptionStatus**
   - **Primary Key**: `SubscriptionStatusId`
   - **Fields**: `Description` (e.g. “Active”, “Cancelled”, etc.)

5. **AccountChangesLog**
   - **Primary Key**: `LogId`
   - **Fields**: `AccountId`, `ChangedField`, `OldValue`, `NewValue`, `Timestamp`
   - Tracks changes made to an Account or its subscription fields (for auditing).

An example of the **relationships**:

- **Account** (1) — (1) **AccountSubscription** — (1) **Subscription**  
- **AccountSubscription** (1) — (1) **AccountSubscriptionStatus**  
- **Account** (1) — (*) **AccountChangesLog** (by referencing `AccountId`)

*(Here, “(1)” and “(*)” just describe cardinality. EF might not strictly enforce one-to-one unless carefully mapped, but conceptually each Account has exactly one `AccountSubscription`, and each `AccountSubscription` references exactly one `SubscriptionStatus`, etc.)*

---

## 5. Repositories

All repository classes implement their respective interfaces under `AccountManager.Interfaces.Repositories`. The repository pattern encapsulates data-access and focuses on:

- `GetAllAsync()`
- `GetByIdAsync()`
- `CreateAsync(...)`
- `UpdateAsync(...)`
- `DeleteAsync(...)`

### 5.1 `AccountRepository`

- **Methods**:
  - `GetAllAsync()` – Returns all accounts with eager-loading for `AccountSubscription -> Subscription -> AccountSubscriptionStatus`.
  - `GetByIdAsync(int id)` – Returns a single account with the same eager-loading.
  - `CreateAsync(Account entity)` – Inserts a new `Account`.
  - `UpdateAsync(Account entity)` – Attaches and updates an existing account, including `AccountSubscription`.
  - `DeleteAsync(int id)` – Removes an account by ID.
  - `SearchAsync(string keyword)` – Searches `CompanyName` for partial matches.

### 5.2 `AccountChangesLogRepository`

- **Methods**:
  - `GetAllAsync()` – Returns logs ordered by descending `Timestamp`.
  - `GetByAccountAsync(int accountId)` – Returns logs for a specific account.
  - `CreateAsync(AccountChangesLog log)` – Inserts a new log entry.
  - `GetByIdAsync(int logId)` – Finds a specific log by primary key.
  - `DeleteAsync(int logId)` – Deletes a log by ID.

### 5.3 `SubscriptionRepository`

- **Methods**:
  - `GetAllAsync()` – Returns all subscription plans.
  - `GetByIdAsync(int id)` – Returns a subscription plan by ID.
  - `CreateAsync(Subscription subscription)` – Inserts a new subscription plan.
  - `UpdateAsync(Subscription subscription)` – Attaches and updates a subscription plan.
  - `DeleteAsync(int id)` – Removes a subscription plan by ID.

### 5.4 `AccountSubscriptionStatusRepository`

- **Methods**:
  - `GetAllAsync()`
  - `GetByIdAsync(int id)`
  - `CreateAsync(AccountSubscriptionStatus status)`
  - `UpdateAsync(AccountSubscriptionStatus status)`
  - `DeleteAsync(int id)`

---

## 6. Services (Business Logic Layer)

Under `AccountManager.Services`, each service implements the corresponding interface from `AccountManager.Interfaces.Services`. Services do the following:

1. Validate inputs / business rules.  
2. Orchestrate repository calls.  
3. Log changes when appropriate.  
4. Possibly throw `InvalidOperationException` if domain rules are broken.

### 6.1 `AccountService`

- **Implements** `IAccountService`
- Depends on:
  - `IAccountRepository` (for account CRUD)
  - `ISubscriptionRepository` (to assign a subscription)
  - `IAccountChangesLogRepository` (to log changes)
- **Key Methods**:
  - `CreateAsync(Account newAccount, int subscriptionId)`
    - Validates subscription existence.
    - Creates an `AccountSubscription` with flags from the chosen subscription.
    - Writes an **"AccountCreated"** log entry.
  - `UpdateAsync(Account updatedAccount)`
    - Checks differences in fields like `CompanyName` or `IsActive`.
    - Logs changes accordingly.
  - `DeleteAsync(int accountId)`
    - Deletes the account and logs an **"AccountDeleted"** entry.
  - `SearchAsync(string keyword)`
    - Delegates to `accountRepo.SearchAsync`.

### 6.2 `AccountChangesLogService`

- **Implements** `IAccountChangesLogService`
- Methods for retrieving logs or deleting them. Also handles creation of new log entries (though mostly used by `AccountService`).

### 6.3 `SubscriptionService`

- **Implements** `ISubscriptionService`
- CRUD for subscription plans.  
- No advanced logic beyond basic create/update.

### 6.4 `AccountSubscriptionStatusService`

- **Implements** `IAccountSubscriptionStatusService`
- CRUD for subscription statuses (e.g. “Active”, “Cancelled”).

---

## 7. API Controllers

In **AccountManager.API**, each controller typically follows the same pattern:

- `[ApiController]`
- `[Route("api/[controller]")]`
- Constructor injection of the relevant service
- CRUD endpoints returning `ActionResult<T>` or `IActionResult`

### 7.1 `AccountsController`

- **Endpoints**:
  - `GET /api/Accounts` → Returns list of all accounts.
  - `GET /api/Accounts/{id}` → Returns a single account by ID.
  - `POST /api/Accounts?subscriptionId={subscriptionId}` → Creates a new account with the specified subscription plan.
  - `PUT /api/Accounts/{id}` → Updates an account.
  - `DELETE /api/Accounts/{id}` → Deletes an account.
  - `GET /api/Accounts/search?q={query}` → Searches accounts by `CompanyName`.

### 7.2 `AccountChangesLogController`

- **Endpoints**:
  - `GET /api/AccountChangesLog` → Retrieves all logs.
  - `GET /api/AccountChangesLog/account/{accountId}` → Logs for a specific account.
  - `POST /api/AccountChangesLog` → Creates a new log entry (rarely called directly, typically used internally).
  - `DELETE /api/AccountChangesLog/{logId}` → Deletes a specific log entry.

### 7.3 `AccountSubscriptionStatusController`

- **Endpoints**:
  - `GET /api/AccountSubscriptionStatus`
  - `GET /api/AccountSubscriptionStatus/{id}`
  - `POST /api/AccountSubscriptionStatus`
  - `PUT /api/AccountSubscriptionStatus/{id}`
  - `DELETE /api/AccountSubscriptionStatus/{id}`

### 7.4 `SubscriptionController`

- **Endpoints**:
  - `GET /api/Subscription`
  - `GET /api/Subscription/{id}`
  - `POST /api/Subscription`
  - `PUT /api/Subscription/{id}`
  - `DELETE /api/Subscription/{id}`

### 7.5 Swagger UI

When running in **Development** mode, the API serves:
- **OpenAPI/Swagger** doc at `/swagger/v1/swagger.json`
- **Interactive Swagger UI** at `/swagger`

---

## 8. Blazor WebAssembly App (AccountManager.APP)

`AccountManager.APP` is a Blazor WASM project that references the same domain entities. It uses `HttpClient` (injected in `Program.cs` or via `[Inject]`) to call the above API endpoints.

### 8.1 Pages

1. **Pages/Accounts.razor**
   - **Route**: `"/accounts"`
   - Lists Accounts with:
     - Searching (debounce logic using a `Timer`).
     - Sorting on columns (ID, CompanyName, IsActive).
     - Buttons to **Edit** or **Delete** each row.
     - A **Create** button linking to `"/accounts/create"`.
   - Uses `Http.GetFromJsonAsync<List<Account>>("api/Accounts")` to load data.

2. **Pages/Accounts.Create.razor**
   - **Route**: `"/accounts/create"`
   - Displays a form to create a new Account (includes selecting a Subscription).
   - Posts data to `"/api/Accounts?subscriptionId={...}"`.
   - On success, navigates back to `"/accounts"`.

3. **Pages/Accounts.Edit.razor**
   - **Route**: `"/accounts/edit/{id:int}"`
   - Loads an existing Account to edit.
   - Displays subscription plan and status in `<select>` elements.
   - Allows toggling fields like `IsActive`, `Is2FAEnabled`, etc.
   - On valid submit, calls `PUT /api/Accounts/{id}`.

4. **Pages/Logs.razor**
   - **Route**: `"/logs"`
   - Fetches all `AccountChangesLog` from `"/api/AccountChangesLog"`.
   - Allows user to search logs by `ChangedField`.
   - Sortable columns: `LogId`, `AccountId`, `ChangedField`, `Timestamp`.

5. **Pages/Subscriptions.razor**
   - **Route**: `"/subscriptions"`
   - Lists all subscription plans with search/sort features.
   - Buttons for **Edit** and **Delete**.
   - Button for **Create New** → `"/subscriptions/create"`.

6. **Pages/Subscriptions.Create.razor**
   - Form to create a new Subscription (fields: `Description`, `IsDefault`, etc.).
   - Posts to `"/api/Subscription"`.

7. **Pages/Subscriptions.Edit.razor**
   - Loads an existing subscription by `id`.
   - Edits fields, then calls `PUT /api/Subscription/{id}`.

### 8.2 Shared Layout / Nav Menu

- A top navigation bar includes links to:
  - `"/accounts"`
  - `"/subscriptions"`
  - `"/logs"`

---

## 9. Business Logic Summary

Overall, **AccountManager** provides:

1. **Account Management**  
   - Create, Update, Delete, View, and Search.  
   - Link to a subscription plan upon creation.  
   - Toggle features like 2FA, IP filtering, Session Timeout if the subscription plan allows it.  

2. **Subscription Plans**  
   - Maintain a library of subscription templates.  
   - Mark a plan as `IsDefault`.  
   - Indicate which features (2FA/IP filter/session) are supported.  

3. **Subscription Status**  
   - Mark an account subscription as Active, Cancelled, etc.  

4. **Change Logging**  
   - Automatic logging of critical account changes (e.g., changing `CompanyName`, toggling `IsActive`, or deleting the account).  
   - Allows historical auditing via `AccountChangesLog`.  

5. **Front-End UI**  
   - A Blazor WebAssembly SPA with pages for each domain entity.  
   - Sorting, searching, and partial updates are demonstrated.  

---

## 10. How to Run & Configure

1. **Database Connection**  
   - Check the `appsettings.json` in **AccountManager.API**. Update `"DefaultConnection"` with your SQL Server details.  
   - Migrate the database if needed:
     ```bash
     cd AccountManager.API
     dotnet ef migrations add InitialCreate
     dotnet ef database update
     ```

2. **Start the API**  
   - In Visual Studio or via CLI:  
     ```bash
     cd AccountManager.API
     dotnet run
     ```
   - By default, it should run on `https://localhost:5001` (SSL) or `http://localhost:5000`.

3. **Start the Blazor WASM client**  
   - In a separate terminal:
     ```bash
     cd AccountManager.APP
     dotnet run
     ```
   - It typically listens on `https://localhost:7247`, but it will proxy or directly call the `API` endpoints (check `Program.cs` or `launchSettings.json` for details).

4. **Access via Browser**  
   - The Blazor site is served at `https://localhost:7247` (or similar).  
   - The API’s Swagger UI is at `https://localhost:5001/swagger`.

---

## 11. Additional Design Notes

- **Dependency Injection**  
  - In `Program.cs` (or `Startup.cs` if using the older template) of `AccountManager.API`, all repositories and services are registered as `Scoped`:
    ```csharp
    builder.Services.AddScoped<IAccountRepository, AccountRepository>();
    builder.Services.AddScoped<IAccountService, AccountService>();
    // etc.
    ```

- **Logging Changes**  
  - In `AccountService`, whenever a property is changed on `UpdateAsync`, it writes an `AccountChangesLog`. This logic can easily be extended to track additional fields.

- **Validation**  
  - Basic validation is provided by `DataAnnotations` (e.g., `[Required]`, `[MaxLength]`).  
  - More advanced validations can be done in services or controllers.

- **Blazor Debounce**  
  - The search text boxes use a `System.Timers.Timer` to delay calling the filtering logic. This prevents excessive calls while typing.

- **Possible Extensions**  
  - Add a real **Authentication/Authorization** layer with Identity or JWT.  
  - Extend subscription logic (e.g. monthly vs yearly pricing).  
  - Implement a custom *Account lifecycle management* (e.g., auto-cancel, renewal dates, etc.).

---

## 12. Conclusion

This **AccountManager** system provides a structured example of:

- **Layered architecture** (Domain, Repository, Services, API, UI).  
- **Entity Framework** for CRUD data operations.  
- **Blazor WebAssembly** for a modern SPA front-end.  
- **Logging / Auditing** changes in the database.  
