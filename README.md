# Smart Event Planner - Backend

A .NET Core Web API backend system designed to help users plan outdoor events by analyzing weather conditions using real-time data from OpenWeatherMap. Built with scalability, maintainability, and clean architecture principles in mind.

## 🚀 Features

### ✅ Core Functionalities
- **Event Management**
  - Create, update, and list events
  - Events include: name, location, date, and event type (e.g., Cricket, Wedding, Hiking)

- **Weather Integration**
  - Fetch current weather and 5-day forecast from OpenWeatherMap
  - Link weather data to specific events
  - Parse and store weather info with internal schema

- **Weather Suitability Analysis**
  - Score weather conditions based on event type (Good / Okay / Poor)
  - Use temperature, precipitation, wind, and condition type

- **Date Recommendations**
  - Recommend alternative dates within the same week for better weather
  - Suggest "optimal" windows for specific outdoor event types

### 💡 Optional Features (In Progress / Planned)
- Hourly weather analysis for event duration
- Smart notifications (email/SMS) for forecast changes
- Historical trend analysis
- Admin dashboard / UI panel (Optional)

---

## ⚙️ Tech Stack

| Layer               | Tech                          |
|--------------------|-------------------------------|
| Backend Framework  | .NET Core Web API             |
| API Integration    | OpenWeatherMap API            |
| ORM                | Entity Framework Core         |
| Database           | PostgreSQL                    |
| Architecture       | Clean Architecture            |
| Deployment         | In Progress (Render/Railway)  |

---

## 📂 Project Structure

SmartEventPlanner/
├── SmartEventPlanner.Api/           # API controllers, middlewares
├── SmartEventPlanner.Application/   # Business logic, use cases
├── SmartEventPlanner.Infrastructure/# EF Core, DBContext, API integrations
├── SmartEventPlanner.sln            # Solution file

📌 API Endpoints
🔹 Event Management
POST /events — Create an event

GET /events — Get list of all events with weather analysis

PUT /events/{id} — Update event details

🔹 Weather Integration
GET /weather/{location}/{date} — Fetch raw weather for given location/date

POST /events/{id}/weather-check — Analyze weather for an event

GET /events/{id}/alternatives — Get better weather dates (optional)

🔹 Analytics
GET /events/{id}/suitability — Get suitability score and weather reasoning
