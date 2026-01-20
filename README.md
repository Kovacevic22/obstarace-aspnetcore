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
/obstarace-aspnetcore
├── backend/ObstaRace.API/   # ASP.NET Core Web API
│   ├── Controllers/         # API Endpoints
│   ├── Data/                # Database Context
│   ├── Dto/                 # Data Transfer Objects
│   ├── Helper/              # Mapping Profiles (AutoMapper)
│   ├── Interfaces/          # Service Interfaces
│   ├── Models/              # Database Entities
│   └── Services/            # Business Logic
│
└── frontend/src/            # React Application
    ├── assets/              # Images (Logo, Hero, etc.)
    ├── components/          # Reusable UI components
    ├── Models/              # TypeScript Types (.type.ts)
    ├── pages/               # Main Application Pages
    └── services/            # API Calls (Axios)
