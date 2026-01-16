# Doctorly.Calender

# 📅 Doctorly Calendar API

A robust, type-safe RESTful API built with .NET 8 for managing medical practitioner schedules. This project demonstrates clean architectural patterns, domain integrity, and automated client generation via NSwag.

## 🏗️ Project Structure
The solution follows a multi-layered architecture to ensure a separation of concerns:
* **Core**: Contains the Rich Domain Model (Entities), business logic, and interface definitions.
* **Infrastructure**: Implements data persistence using Entity Framework Core and SQLite.
* **Application Services**: Orchestrates the flow between the API and the Domain, handling DTO mapping and coordination.
* **API**: The presentation layer containing Controllers, Middleware, and OpenAPI configurations.

## 📦 Technology Stack & NuGet Packages
* **ASP.NET Core 8**: The high-performance framework for building the REST API.
* **Entity Framework Core (SQLite)**: Chosen for its portability and ease of review (file-based database).
* **FluentValidation**: Decouples validation logic from DTOs to ensure high data quality.
* **NSwag.AspNetCore**: Used for OpenAPI (Swagger) documentation and automated Client SDK generation.
* **xUnit**: The primary testing framework for verifying domain logic.

---

## 🚀 Getting Started

### Initial Database Setup
Before running the application, you must initialize the SQLite database schema to ensure the tables exist:

1.  Open the **Package Manager Console** in Visual Studio.
2.  Install the migration tools:
    `Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.0`
3.  Generate the initial migration files:
    `Add-Migration InitialCreate`
4.  Apply the schema to the local database file:
    `Update-Database`

### How to Run
1.  Set `Doctorly.Calendar` as the **Startup Project**.
2.  Press `F5` or click **Run**.
3.  The Swagger UI will launch at `http://localhost:[PORT]/swagger`, where you can interact with the API.

---

## 🧠 Architectural Concepts & Best Practices

### 1. DTO to Domain Mapping
We use **Records** for DTOs to ensure immutability during the request-response lifecycle. These are mapped to **Entities** in the service layer. This ensures that business rules (like schedule validation) stay within the `CalendarEvent` entity, preventing an "Anemic Domain Model."

### 2. Global Exception Handling
A custom middleware is implemented to catch all exceptions globally. This ensures the API returns a consistent JSON error format. It specifically looks for `DomainException` types to return `400 Bad Request` messages while keeping technical stack traces hidden.

### 3. Client Generation (NSwag)
This project uses **NSwag** instead of standard Swashbuckle to facilitate automated client generation.
* **OpenAPI Spec**: Available at `/swagger/v1/swagger.json`.
* **Automated SDK**: By utilizing the `nswag.json` configuration, developers can generate type-safe TypeScript or C# clients, ensuring the frontend is always in sync with the backend.

### 4. Cascade Deletes
The database is configured with **Referential Integrity**. When a `CalendarEvent` is deleted via the API, all associated `Attendee` records are automatically removed by the database, preventing orphaned data.

---

## 📡 API Testing Guide (Sample Payloads)

### 1. Create a New Event (POST)
**Endpoint**: `POST /api/Events`
**Payload**:
```json
{
  "title": "Evening Ward Rounds",
  "description": "Daily check-in on recovery ward patients.",
  "startTime": "2026-06-20T18:00:00Z",
  "endTime": "2026-06-20T19:30:00Z",
  "attendees": [
    {
      "name": "Dr. Sarah Jenkins",
      "email": "s.jenkins@doctorly.com",
      "isAttending": true
    }
  ]
}