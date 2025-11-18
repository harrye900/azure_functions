# Azure Functions .NET Application

## Local Development

1. Install .NET 8 SDK:
   Download from https://dotnet.microsoft.com/download/dotnet/8.0

2. Install Azure Functions Core Tools:
   ```bash
   npm install -g azure-functions-core-tools@4 --unsafe-perm true
   ```

3. Restore dependencies:
   ```bash
   dotnet restore
   ```

4. Run locally:
   ```bash
   func start
   ```

5. Test the function:
   ```bash
   curl "http://localhost:7071/api/HttpTrigger?name=World"
   ```

## Deployment to Azure

### Prerequisites
- Azure CLI installed
- .NET 8 SDK installed
- Azure subscription

### Steps

1. Login to Azure:
   ```bash
   az login
   ```

2. Create resource group:
   ```bash
   az group create --name myResourceGroup --location eastus
   ```

3. Create storage account:
   ```bash
   az storage account create --name mystorageaccount --location eastus --resource-group myResourceGroup --sku Standard_LRS
   ```

4. Create function app (Flex Consumption):
   ```bash
   az functionapp create --resource-group myResourceGroup --name myFunctionApp --storage-account mystorageaccount --runtime dotnet-isolated --runtime-version 8.0 --functions-version 4 --flexconsumption-location eastus
   ```

5. Deploy the function:
   ```bash
   func azure functionapp publish myFunctionApp
   ```

## CI/CD with GitHub Actions

### Setup Automated Deployment

1. Get publish profile from Azure:
   ```bash
   az functionapp deployment list-publishing-profiles --name myFunctionApp --resource-group myResourceGroup --xml
   ```

2. Add GitHub secret:
   - Go to GitHub repo → Settings → Secrets → Actions
   - Add `AZURE_FUNCTIONAPP_PUBLISH_PROFILE` with the XML content

3. Update workflow file:
   - Change `AZURE_FUNCTIONAPP_NAME` in `.github/workflows/deploy.yml`

4. Push to main branch to trigger deployment

## Function Details

- **Runtime**: .NET 8 (Isolated)
- **Trigger**: HTTP (GET/POST)
- **Input**: Query parameter or JSON body with "name" field
- **Output**: JSON response with greeting message
- **URL**: `https://myFunctionApp.azurewebsites.net/api/HttpTrigger?name=YourName`