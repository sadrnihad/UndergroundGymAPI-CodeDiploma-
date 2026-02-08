# Underground Gym — Full-Stack ASP.NET Core 8 Website

A professional gym website with a complete **HTML/CSS/JavaScript frontend** and **C# ASP.NET Core 8 REST API backend**, built for Visual Studio 2022.

When you press **F5**, the full Underground Gym website opens in your browser — not just Swagger.

---

## Quick Start

### Prerequisites
- **Visual Studio 2022** (17.8+) with the "ASP.NET and web development" workload
- **.NET 8.0 SDK** ([download](https://dotnet.microsoft.com/download/dotnet/8.0))

### Run the Project
1. Open `UndergroundGymAPI.sln` in Visual Studio 2022
2. Press **F5** (or Ctrl+F5 for without debugging)
3. The **Underground Gym website** opens automatically in your browser
4. The API uses **InMemory database** by default — no SQL Server setup needed
5. Sample data (trainers, classes, plans, testimonials) is seeded automatically

### Access Points
| URL | What You See |
|-----|-------------|
| `https://localhost:7001/` | **Full gym website** (home page) |
| `https://localhost:7001/schedule.html` | Class schedule with booking |
| `https://localhost:7001/membership.html` | Membership plans |
| `https://localhost:7001/admin.html` | Admin dashboard |
| `https://localhost:7001/swagger` | Swagger API documentation |

---

## Website Pages

### Home Page (`index.html`)
- Hero section with "Forge Your Legacy" branding
- Stats bar (500+ members, 12+ classes, 4 trainers, 24/7 access)
- Classes preview loaded from API
- Membership plans loaded from API
- Trainer profiles loaded from API
- Gallery showcasing gym facilities
- Testimonials from members
- Contact form with location info

### Schedule Page (`schedule.html`)
- Full class schedule table loaded from API
- Filter by class type (Strength, HIIT, Boxing, Yoga, etc.)
- Book classes directly from the table
- Shows available spots and capacity

### Membership Page (`membership.html`)
- All membership plans loaded from API
- Feature comparison for each plan
- Subscribe button for each plan
- "Why Underground" benefits section

### Admin Dashboard (`admin.html`)
- Overview with key statistics
- Manage Classes (view, add, delete)
- Manage Bookings (view all bookings)
- Manage Trainers (view, delete)
- Manage Memberships (view subscriptions)
- Contact Messages (view submissions)
- Testimonials management

---

## Switch to SQL Server (Optional)
Update `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=UndergroundGymDb;Trusted_Connection=True;"
  }
}
```
Then run EF Core migrations:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## API Endpoints

All API endpoints are under `/api/` and can be tested via Swagger at `/swagger`.

### Classes
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/classes` | Get all classes |
| GET | `/api/classes/{id}` | Get class by ID |
| POST | `/api/classes` | Create a new class |
| PUT | `/api/classes/{id}` | Update a class |
| DELETE | `/api/classes/{id}` | Soft-delete a class |

### Membership Plans
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/memberships/plans` | Get all plans |
| POST | `/api/memberships/plans` | Create a plan |
| POST | `/api/memberships/subscribe` | Subscribe user to a plan |
| GET | `/api/memberships/subscriptions` | Get all subscriptions |

### Trainers
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/trainers` | Get all trainers |
| POST | `/api/trainers` | Create a trainer |
| DELETE | `/api/trainers/{id}` | Soft-delete a trainer |

### Bookings
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/bookings` | Get all bookings |
| POST | `/api/bookings` | Create a booking |
| DELETE | `/api/bookings/{id}` | Cancel a booking |

### Testimonials, Gallery, Contact, Dashboard
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/testimonials` | Get testimonials |
| GET | `/api/gallery` | Get gallery images |
| POST | `/api/contact` | Submit contact form |
| GET | `/api/dashboard/stats` | Dashboard statistics |

---

## Project Structure

```
UndergroundGymAPI/
├── wwwroot/                  # Frontend (served automatically)
│   ├── css/style.css         # Yellow/black gym theme
│   ├── js/app.js             # API calls and interactivity
│   ├── images/               # Image assets
│   ├── index.html            # Home page
│   ├── schedule.html         # Class schedule
│   ├── membership.html       # Membership plans
│   └── admin.html            # Admin dashboard
├── Controllers/              # API controllers
├── Models/                   # Entity models
├── DTOs/                     # Data Transfer Objects
├── Data/                     # Database context and seeder
├── Services/                 # Business logic layer
├── Properties/
│   └── launchSettings.json
├── Program.cs                # Entry point (serves frontend + API)
├── appsettings.json
└── UndergroundGymAPI.sln     # Visual Studio solution
```

---

## Architecture

- **Static Files Middleware** serves the frontend from `wwwroot/`
- **Clean Architecture** with Controllers → Services → Data separation
- **Dependency Injection** for all services
- **Entity Framework Core** with InMemory or SQL Server
- **Swagger/OpenAPI** at `/swagger` for API testing
- **Soft Delete** for all entities

---

## License
MIT — Built for Underground Gym.
