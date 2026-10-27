# Scantry 🥫

Scantry is an N-tier ASP.NET Core MVC web application designed to help households manage their pantry inventory collaboratively, track products, and share access securely via unique tokens.

## 🚀 Current Features

### Household Management:
* Create custom households with unique, auto-generated GUID access tokens.
* View a personalized dashboard listing all households you belong to.
* One-click copy functionality for access tokens with visual feedback.

### Secure Collaboration:
* Join existing households seamlessly using a shared GUID access token.
* Role and membership isolation mapped through robust database relationships.

### Smart Inventory & Barcode Scanner:
* **Camera Barcode Scanning:** Built-in mobile-friendly camera scanner (powered by `Html5-Qrcode`) to scan household items instantly.
* **External API Auto-Enrichment:** Automatically queries open community databases (**Open Beauty Facts** & **Open Food Facts**) when a scanned barcode isn't found locally, instantly pulling brand, name, and product details on the fly.
* **Expiration & Low Stock Tracking:** Visual status indicators for fresh, expiring, and low-stock items.
* **Favorites & Quick Management:** Quick-toggle favorites, compact card layouts, and universal delete modals.

### Authentication & Security:
* Powered by ASP.NET Core Identity for user registration and login.
* Role-based and authentication-guarded navigation links.

## 🛠️ Tech Stack & Architecture

* **Framework:** ASP.NET Core MVC (.NET)
* **Data Access:** Entity Framework Core (EF Core) with SQL Server
* **External Integrations:** Open Food Facts & Open Beauty Facts APIs via `HttpClient`
* **Architecture:** Multi-layered N-tier structure:
  * `Scantry.Core`: Business logic, services, DTOs, and view models.
  * `Scantry.Data`: Database context, entities, and repository pattern implementation.
  * `Scantry.Web`: Controllers, UI views (Razor), camera scanner scripts, and frontend assets.
* **Frontend:** Bootstrap, HTML5, CSS3, JavaScript, and Bootstrap Icons.

## 📂 Database Schema Overview

* **ApplicationUser:** Manages system authentication via ASP.NET Core Identity.
* **Household:** Stores household details and unique join tokens (`AccessToken`).
* **HouseholdUsers:** Mapping table establishing a many-to-many relationship between users and households.
* **HouseholdProducts:** Relational framework for tracking global catalogue items within specific household inventories, quantities, expiration dates, and custom minimum thresholds.
* **Product:** Global product catalogue storing barcodes, brands, model names, and sizes.

## 🏁 Getting Started (Local Development)
1. Clone the repository:
2. Bash
`git clone https://github.com/your-username/Scantry.git`
3. Update your connection string in `appsettings.json` to point to your local SQL Server instance.
4. Apply database migrations using the NuGet Package Manager Console or CLI:
5. Bash
`dotnet ef database update`
6. Run the application and register an account to start managing households!
