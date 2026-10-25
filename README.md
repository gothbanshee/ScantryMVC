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

### Authentication & Security:
* Powered by ASP.NET Core Identity for user registration and login.
* Role-based and authentication-guarded navigation links.

## 🛠️ Tech Stack & Architecture
### Framework: ASP.NET Core MVC (.NET)

### Data Access: Entity Framework Core (EF Core) with SQL Server

### Architecture: Multi-layered N-tier structure:

* Scantry.Core: Business logic, services, and view models.
* Scantry.Data: Database context, entities, and repository pattern implementation.
* Scantry.Web: Controllers, UI views (Razor), and frontend assets.

### Frontend: Bootstrap, HTML5, CSS3, and vanilla JavaScript (with responsive inline SVG iconography).

## 📂 Database Schema Overview
* ApplicationUser: Manages system authentication via ASP.NET Core Identity.
* Household: Stores household details and unique join tokens (AccessToken).
* HouseholdUsers: Mapping table establishing a many-to-many relationship between users and households.
* HouseholdProducts: Relational framework for tracking global catalogue items within specific household inventories.
* Product: Global product catalogue

## 🏁 Getting Started (Local Development)
1. Clone the repository:
2. Bash
`git clone https://github.com/your-username/Scantry.git`
3. Update your connection string in `appsettings.json` to point to your local SQL Server instance.
4. Apply database migrations using the NuGet Package Manager Console or CLI:
5. Bash
`dotnet ef database update`
6. Run the application and register an account to start managing households!
