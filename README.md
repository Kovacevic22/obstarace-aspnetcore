# 🏃 ObstaRace
A web application for managing and organizing obstacle races. Users can join races, organizers can create events, and admins manage the system.
---
## Tech Stack
**Backend:**
* C# / .NET 10 Web API
* Entity Framework Core (SQL Database)
* JWT Authentication (HttpOnly Cookies)
**Frontend:**
* React.js
* Axios
* Tailwind CSS
---
---
## Features
### For Participants
* **Browse Races:** Find races by typing the name, or filter by difficulty and distance.
* **Register:** Sign up for races easily.
* **Stats:** Track your finished races.
* **Automated Notifications:** Receive email confirmations upon registration, status updates (Approved/Rejected) from organizers, and a reminder 7 days before the race.
### For Organisers
* **Create Races:** Add new events and obstacles.
* **Manage:** View and approve participants.
* **Dashboard:** See stats for your races.
### For Admins
* **Full Access:** Create and delete any race or obstacle.
* **Control:** Approve new organizers.
* **Users:** Ban/Unban users if needed.

---

## Technical Overview

**Architecture (4-Layer):**
- Presentation Layer (API): Controllers, middleware, and dependency injection
- Application Layer: Business logic services and DTOs
- Infrastructure Layer: Data access repositories and external services
- Domain Layer: Core entities and business models

**Security:**
- JWT authentication with HttpOnly cookies
- BCrypt password hashing
- Rate limiting on authentication endpoints (brute-force protection)
- Role-based authorization (Admin, Organiser, Participant)

**Performance Optimizations:**
- Async streaming with yield for memory-efficient bulk operations
- Repository pattern with Entity Framework Core
- Pagination on all data listings

**Background Services:**
- Automatic race status updates (hourly)
- Email reminder system (7 days before race)
- Completion notifications when race finishes

**Error Handling:**
- Global exception handler with ProblemDetails response format
- Custom status code pages for 401/403 responses

---
## Project Structure
The project is organized into two main parts:
```text
/obstarace-application
├── backend/
│   ├── ObstaRace.API/                # Presentation Layer
│   │   ├── Controllers/              # API Endpoints
│   │   ├── Middleware/               # Global Exception Handler
│   │   ├── appsettings.json          # Configuration & JWT Settings
│   │   └── Program.cs                # Dependency Injection & Middleware
│   │
│   ├── ObstaRace.Application/        # Application Layer (Business Logic)
│   │   ├── Services/                 # Business Logic Implementation
│   │   ├── Dto/                      # Data Transfer Objects
│   │   ├── Interfaces/               # Service Interfaces
│   │   └── Helper/                   # AutoMapper Profiles
│   │
│   ├── ObstaRace.Infrastructure/     # Infrastructure Layer (Data Access & External Services)
│   │   ├── Configuration/            # Email and Reminder settings (IOptions)
│   │   ├── Data/                     # DbContext
│   │   ├── EmailTemplates/           # HTML templates for automated emails
│   │   ├── Migrations/               # Database Migrations
│   │   ├── Repository/               # Repository Pattern Implementation
│   │   └── Service/                  # EmailService & RaceReminderBgService
│   │
│   └── ObstaRace.Domain/             # Domain Layer (Core)
│       └── Models/                   # Database Entities
│
└── frontend/
    └── src/                          # React Application
        ├── assets/                   # Static files (Images, Logos)
        ├── components/               # Reusable UI Components
        ├── context/                  # React Context Providers
        ├── hooks/                    # Custom React Hooks
        ├── Models/                   # TypeScript Types/Interfaces
        ├── pages/                    # Main Application Views
        ├── routes/                   # Route Definitions
        ├── services/                 # API Calls (Axios/Fetch)
        └── utils/                    # Helper Functions
```
---
## Class Diagram

<p align="center">
  <img src="https://i.imgur.com/kIMXZ85.png" alt="Dashboard Preview" width="100%">
</p>

---
## Installation

### Prerequisites
* .NET 10 SDK
* Node.js and npm
* SQL Server / SQL Server Express
* Git

### Backend Setup
```bash
# Navigate to backend directory
cd backend/ObstaRace

# Copy the example config and fill in your values
cp appsettings.example.json appsettings.json
```

Open `appsettings.json` and fill in:
* **ConnectionStrings** – your SQL Server connection string
* **Jwt.Key** – a random secret key (min. 32 characters)
* **EmailSettings** – your SMTP credentials
```bash
# Restore dependencies
dotnet restore

# Apply database migrations
dotnet ef database update --project ObstaRace.Infrastructure --startup-project ObstaRace.API

# Run the API
dotnet run
```

The API will run on `https://localhost:5235` by default.

### Frontend Setup
```bash
# Navigate to frontend directory
cd frontend

# Install dependencies
npm install

# Start development server
npm run dev
```

The frontend will run on `http://localhost:5173` by default.
