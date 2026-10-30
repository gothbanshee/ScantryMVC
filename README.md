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

## Screenshots

### Homepage
  <img width="1838" height="960" alt="image" src="https://github.com/user-attachments/assets/c54babb5-7286-4d1b-8b68-6beac8a61efc" />

### Register and Login
  <img width="1853" height="959" alt="image" src="https://github.com/user-attachments/assets/d06274f8-9ce9-4dde-85d7-f50f4e85dec0" />
  <img width="1853" height="959" alt="image" src="https://github.com/user-attachments/assets/5af02f41-c3bc-4313-8795-d0e9150a7423" />

### Create or join a household - create or join as many households as you need!
  <img width="1853" height="959" alt="image" src="https://github.com/user-attachments/assets/b2d1b2b1-e229-4551-932d-6d72c624e140" />

### My Households - here you can find and manage all households that you are a member in.
<img width="1853" height="959" alt="image" src="https://github.com/user-attachments/assets/a836e8ba-a5fc-46d6-9fb6-e395a6edd798" />

* **Sharing:**
  * Directly share a joining link for the household in an app of your choice. Every registered user will join the household after clicking the link. If the user isn't logged in, they will be redirected to the log-in page.  
  * See the current members of the household

* **Household name**
  
* **Manage**
  * See household details - all products, favourites, expiring, low-stock items
  * Rename household
  * Leave household
  * Delete household and all of its information
<img width="1853" height="959" alt="image" src="https://github.com/user-attachments/assets/284364b7-3d82-4057-9d47-b8f138ec04a2" />

### Household details - here you can find all your scanned items, favourites, expiring products (where applicable), low-stock items. You also have the button for sharing the joining link again.
<img width="1853" height="959" alt="image" src="https://github.com/user-attachments/assets/eaa5e07e-6e48-4b9e-90b4-0b0bf1537818" />

### Local product catalogue - full list of all items that any user ever scanned, along with the basic information for them.
<img width="1853" height="959" alt="image" src="https://github.com/user-attachments/assets/a29b9bd9-e343-45fe-93de-ce6541febfc8" />

* Editing product info in the local catalogue - The catalogue will be only shown to the admins after the next update. Users will only be able to report wrong product information, which will alert admins, so that they can check and edit it, if needed. Right now every user can edit the items. Deleting items is not implemented on purpose.
<img width="1853" height="959" alt="image" src="https://github.com/user-attachments/assets/7e380e63-9375-4a21-8774-e8236174a8f8" />

### Scanning new products - The scanning button "Add new product" is available in the local product catalogue and also in all of the lists in a household's details.
* You can either scan a product's barcode with your device's camera or manually enter the barcode. The app looks for the barcode in the local product catalogue first, and if not found, searches in external APIs.
  * Upon scanning when in a household's details, the product is automatically added to the local catalogue (if not already existing) and also to the household.
    * If you scan or enter the barcode of an item that is already in your household's list, it's quantity is automatically increased by 1.
  * Upon scanning from the local product catalogue, the product is only added to the local catalogue.

<img width="1853" height="959" alt="image" src="https://github.com/user-attachments/assets/5408dcc9-bada-40c4-ba4f-45a78cfddb48" />

<img width="1853" height="959" alt="image" src="https://github.com/user-attachments/assets/85617a0a-eb86-42ec-b776-e790c28dbe77" />

### Household details
<img width="1853" height="959" alt="image" src="https://github.com/user-attachments/assets/eac2e5b6-6b59-4e22-9f24-37dda08ecaa3" />
* **All Items** - displays all products added for this household, along with their current quantity and minimum threshold. Default value after scanning a new item for the household is 1.
