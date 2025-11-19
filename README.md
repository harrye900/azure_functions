# Task Manager - Azure Functions Web Application

A complete web application built with Azure Functions that demonstrates how to create a full-stack application using serverless architecture.

## Features

- ✅ **Complete REST API** for task management
- ✅ **Web Interface** with HTML/CSS/JavaScript
- ✅ **CRUD Operations** (Create, Read, Update, Delete)
- ✅ **Responsive Design** works on mobile and desktop
- ✅ **Health Check** endpoint for monitoring
- ✅ **Serverless Architecture** with Azure Functions

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/` | Web application home page |
| GET | `/api/health` | Health check |
| GET | `/api/tasks` | Get all tasks |
| GET | `/api/tasks/{id}` | Get specific task |
| POST | `/api/tasks` | Create new task |
| PUT | `/api/tasks/{id}` | Update task |
| DELETE | `/api/tasks/{id}` | Delete task |

## Local Development

1. **Install .NET 8 SDK**:
   Download from https://dotnet.microsoft.com/download/dotnet/8.0

2. **Install Azure Functions Core Tools**:
   ```bash
   npm install -g azure-functions-core-tools@4 --unsafe-perm true
   ```

3. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

4. **Run locally**:
   ```bash
   func start
   ```

5. **Open in browser**:
   ```
   http://localhost:7071
   ```

## Deployment to Azure

### Create Function App
```bash
az functionapp create --resource-group test --name racetrac001-dotnet --storage-account racetrac001 --runtime dotnet-isolated --runtime-version 8.0 --functions-version 4 --flexconsumption-location eastus
```

### Deploy
```bash
dotnet publish --configuration Release
func azure functionapp publish racetrac001-dotnet
```

## Usage Examples

### Create Task (API)
```bash
curl -X POST "https://racetrac001-dotnet.azurewebsites.net/api/tasks" \
-H "Content-Type: application/json" \
-d '{"title":"Learn Azure Functions","description":"Build a complete web app"}'
```

### Get All Tasks (API)
```bash
curl "https://racetrac001-dotnet.azurewebsites.net/api/tasks"
```

### Web Interface
Simply visit: `https://racetrac001-dotnet.azurewebsites.net`

## Architecture

```
Frontend (HTML/JS) → Azure Functions → In-Memory Storage
```

- **Frontend**: Single-page application with vanilla JavaScript
- **Backend**: Azure Functions with HTTP triggers
- **Storage**: In-memory (for demo - use Azure SQL/Cosmos DB for production)
- **Hosting**: Azure Functions Flex Consumption plan

## Production Considerations

For production applications, consider:
- **Database**: Azure SQL Database or Cosmos DB instead of in-memory storage
- **Authentication**: Azure AD B2C or custom authentication
- **Caching**: Azure Redis Cache for better performance
- **Monitoring**: Application Insights for detailed telemetry
- **Security**: API Management for rate limiting and security