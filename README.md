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
### For Organisers
* **Create Races:** Add new events and obstacles.
* **Manage:** View and approve participants.
* **Dashboard:** See stats for your races.
### For Admins
* **Full Access:** Create and delete any race or obstacle.
* **Control:** Approve new organizers.
* **Users:** Ban/Unban users if needed.
---
## Project Structure
The project is organized into two main parts:
```text
/obstarace-application
├── backend/
│   ├── ObstaRace.API/                # Presentation Layer
│   │   ├── Controllers/              # API Endpoints
│   │   ├── appsettings.json          # Configuration & JWT Settings
│   │   └── Program.cs                # Dependency Injection & Middleware
│   │
│   ├── ObstaRace.Application/        # Application Layer (Business Logic)
│   │   ├── Services/                 # Business Logic Implementation
│   │   ├── Dto/                      # Data Transfer Objects
│   │   ├── Interfaces/               # Service Interfaces
│   │   └── Helper/                   # AutoMapper Profiles
│   │
│   ├── ObstaRace.Infrastructure/     # Infrastructure Layer (Data Access)
│   │   ├── Data/                     # DbContext (EF Core)
│   │   ├── Repository/               # Repository Pattern Implementation
│   │   └── Migrations/               # Database Migrations
│   │
│   └── ObstaRace.Domain/             # Domain Layer (Core)
│       └── Models/                   # Database Entities
│
└── frontend/
    └── src/                          # React Application
        ├── assets/                   # Static files (Images, Logos)
        ├── components/               # Reusable UI Components
        ├── Models/                   # TypeScript Types/Interfaces
        ├── pages/                    # Main Application Views
        └── services/                 # API Calls (Axios/Fetch)
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
* Git

### Backend Setup
```bash
# Navigate to backend directory
cd backend/ObstaRace.API

# Restore dependencies
dotnet restore

# Apply database migrations
dotnet ef database update --project ../ObstaRace.Infrastructure

# Run the API
dotnet run
```

The API will run on `https://localhost:5001` by default.

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

### Configuration
Update `appsettings.json` in `ObstaRace.API` with your:
* Database connection string
* JWT secret key
* CORS origins if needed
