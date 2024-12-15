# CandidatePortal Project

## Overview

CandidatePortal is a robust and user-friendly web application designed for managing candidate records. The application provides a React-based admin dashboard to interact with a .NET Core backend API. It includes features such as candidate registration, resume management, statistics visualization, and monitoring setup.

### Technologies Used

- **Backend**: ASP.NET Core 6
- **Frontend**: React.js
- **Database**: SQL Server
- **Monitoring**: Serilog for logging
- **Containerization**: Docker

---

## Features

1. **Admin Dashboard**:

   - View, register, and manage candidates.
   - Download resumes.
   - View statistics, including total candidates and department-wise breakdown.

2. **Backend API**:

   - Secure endpoints for candidate management.
   - Health check and monitoring support.

3. **Logging**:

   - Structured logging using Serilog (console and file logs).

4. **Statistics**:

   - Total candidates and department distribution via API and dashboard.

5. **Docker Support** (in progress):

   - Containerized backend and frontend for easy deployment.

---

## Setup Instructions

### Prerequisites

1. Install the following:

   - .NET SDK (v6 or later)
   - Node.js (v16 or later) and npm
   - SQL Server (local or cloud)
   - Docker (optional, for containerization)

2. Clone the repository:

   ```bash
   git clone <repository-url>
   cd CandidatePortal
   ```

---

### Backend Setup

1. Navigate to the backend folder:

   ```bash
   cd CandidatePortal
   ```

2. Update the database connection string in `appsettings.Development.json`:

   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=CandidatePortal;Trusted_Connection=True;"
   }
   ```

3. Apply migrations and seed the database:

   ```bash
   dotnet ef database update
   ```

4. Run the backend application:

   ```bash
   dotnet run
   ```

5. Verify the API:

   - Swagger UI: [http://localhost/swagger](http://localhost/swagger)
---

### Frontend Setup

1. Navigate to the frontend folder:

   ```bash
   cd admin-dashboard
   ```

2. Install dependencies:

   ```bash
   npm install
   ```

3. Start the React app:

   ```bash
   npm start
   ```

4. Access the admin dashboard: [http://localhost:3000](http://localhost:3000)

---

### Running with Docker (in progress)

1. Build and run the containers:

   ```bash
   docker-compose up --build
   ```

2. Access the application:

   - Backend: [http://localhost](http://localhost)
   - Frontend: [http://localhost:3000](http://localhost:3000)

---

## Usage Instructions

### Admin Dashboard

1. **Candidate Management**:

   - View a list of candidates with pagination.
   - Download resumes directly from the dashboard.

2. **Statistics Page**:

   - Navigate to the Statistics page using the "View Statistics" button.
   - View total candidates and department-wise distribution.

---

## Advanced Details

### Logging

- Logs are written to the console and the `logs` folder.
- Customize Serilog configuration in `Program.cs`.

### Monitoring

- Extend monitoring with Prometheus and Grafana if needed.

---



