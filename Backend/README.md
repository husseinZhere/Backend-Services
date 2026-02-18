# Backend Services

This folder contains all backend services for the PulseX graduation project.

## Structure

```
Backend/
├── PulseX.API/              # ASP.NET Core Web API (Controllers, Services)
├── PulseX.Core/             # Core domain models, DTOs, interfaces
├── PulseX.Data/             # Data access layer with EF Core
├── ai-service/              # Python AI service (Chatbot & X-Ray analysis)
├── PulseX.slnx              # .NET Solution file
├── global.json              # .NET SDK configuration
├── appsettings.example.json # Configuration template
├── *.sql                    # Database migration scripts
└── README.md                # This file
```

## Getting Started

### .NET Backend

1. Navigate to the Backend folder:
   ```bash
   cd Backend
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the solution:
   ```bash
   dotnet build PulseX.API/PulseX.API.csproj
   ```

4. Run the API:
   ```bash
   cd PulseX.API
   dotnet run
   ```

### AI Service

1. Navigate to the AI service folder:
   ```bash
   cd Backend/ai-service
   ```

2. Install dependencies:
   ```bash
   pip install -r requirements.txt
   ```

3. Run the service:
   ```bash
   python main.py
   ```

For more details, see the README files in each subfolder.
