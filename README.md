# LifeCycleProj

## Overview
`LifeCycleProj` is a .NET 10 web application for importing CSV data and storing a production structure in SQL Server.

The application ingests CSV rows and maps them into a hierarchy:

- `Location` → `Line` → `Machine` → `Component` → `Article`

It also updates existing component quantities when matching records already exist.

---

## Solution Structure

The solution contains 3 projects:

- `LifeCycle`  
  ASP.NET Core web app (UI + application orchestration)
- `LifeCycle.DataAccess`  
  Entity Framework Core context, migrations, and Unit of Work
- `LifeCycle.Models`  
  Domain models and CSV data objects/mappings

---

## Frameworks & Libraries

### Platform
- `.NET 10` (`net10.0`)

### Backend / Web
- `ASP.NET Core` with controllers and Razor views (`AddControllersWithViews`)
- `Entity Framework Core 10`
- `Microsoft.EntityFrameworkCore.SqlServer`

### Data Import
- `CsvHelper` for CSV parsing and column mapping

### Frontend
- Razor view engine (`.cshtml`)
- `Bootstrap`
- `jQuery`

---

## High-Level Architecture

The app follows a layered structure:

1. **Presentation Layer (`LifeCycle`)**
   - `HomeController` receives uploaded CSV files
   - Razor views render the upload UI and messages

2. **Service Layer (`LifeCycle.Services`)**
   - `ICsvImportService` / `CSVImportService`
   - Handles CSV parsing, validation, and import workflow

3. **Data Access Layer (`LifeCycle.DataAccess`)**
   - `ApplicationDbContext` with EF Core `DbSet<>` entities
   - `UnitOfWork` abstraction for saving and transaction handling

4. **Domain Layer (`LifeCycle.Models`)**
   - Entity models: `Location`, `Line`, `Machine`, `Component`, `Article`
   - DTO/mapping classes for CSV import (`CsvImportDO`, `CsvMappingDO`)

---

## Request/Data Flow

1. User uploads a CSV file from the Home page.
2. `HomeController.ImportCSV` sends stream to `ICsvImportService`.
3. `CSVImportService`:
   - reads CSV using `CsvHelper`
   - maps columns via `CsvMappingDO`
   - creates/fetches related entities in order:
     `Location` → `Line` → `Machine` → `Article` → `Component`
   - updates quantity when component already exists
4. Changes are persisted through EF Core + Unit of Work transaction logic.
5. UI shows success/error result via `TempData`.

---

## Database
<img width="1905" height="1343" alt="WhatsApp Image 2026-08-24 at 18 48 41" src="https://github.com/user-attachments/assets/fb08c72a-7af8-4b13-9164-107caedb4f57" />
<img width="393" height="488" alt="image" src="https://github.com/user-attachments/assets/32c71665-dac1-4e2f-8dda-3f00513b381d" />

- Provider: `SQL Server`
- Configured via `ConnectionStrings:DefaultConnection` in `appsettings.json`
- `ApplicationDbContext` contains:
  - `Components`
  - `Locations`
  - `Lines`
  - `Machines`
  - `Articles`

---

## Run the Project
<img width="1919" height="992" alt="Screenshot 2026-08-24 185031" src="https://github.com/user-attachments/assets/6e3a5afb-4767-4edf-af00-70dcfe1bf2dc" />
<img width="1919" height="991" alt="Screenshot 2026-08-24 183911" src="https://github.com/user-attachments/assets/83a95b43-a900-4982-ad0d-a5c3435546a1" />
<img width="927" height="702" alt="image" src="https://github.com/user-attachments/assets/fa459957-dcc5-41ef-b4da-dba5f3505251" />


### Prerequisites
- .NET 10 SDK
- SQL Server instance

### Steps
1. Configure connection string in `LifeCycle/appsettings.json`.
2. Apply migrations (if needed).
3. Run the web project (`LifeCycle`) from Visual Studio or CLI.

---

## Current Notes

- UI is currently focused on CSV import through the Home page.
- Import column names are expected in German (e.g., `Ordner`, `Artikelnummer`, `Menge`).
- The app currently uses controller-based Razor views (MVC style).
