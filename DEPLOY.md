# Deployment Guide - Azure App Service

## Why Azure App Service?
- ✅ **Native .NET support** - No Docker needed
- ✅ **Industry standard** - Microsoft's official platform
- ✅ **Free tier available** - F1 tier for development
- ✅ **GitHub integration** - Auto-deploy from GitHub
- ✅ **Zero configuration** - Works out of the box

## Quick Deployment Steps

### 1. Create Azure App Service (5 minutes)

1. Go to https://portal.azure.com
2. Click "Create a resource" → Search "Web App"
3. Click "Create"
4. Fill in:
   - **Subscription**: Free Trial (or your subscription)
   - **Resource Group**: Create new (e.g., `ski-field-tracker`)
   - **Name**: `ski-field-tracker-api` (must be globally unique)
   - **Publish**: Code
   - **Runtime stack**: `.NET 8 (LTS)`
   - **Operating System**: Linux
   - **Region**: Choose closest to you (e.g., East US)
   - **App Service Plan**: 
     - Click "Create new"
     - Name: `ski-field-tracker-plan`
     - **Sku and size**: **Free F1** (for development) or **Basic B1** ($13/month for production)
5. Click "Review + create" → "Create"
6. Wait for deployment (~2-3 minutes)

### 2. Configure Environment Variables

1. In Azure Portal, go to your App Service
2. Left menu → **Configuration** → **Application settings**
3. Add:
   - `ConnectionStrings__DefaultConnection` = Your Supabase connection string
   - `ASPNETCORE_ENVIRONMENT` = `Production`
4. Click "Save"

### 3. Configure GitHub Deployment

1. In Azure Portal, go to your App Service
2. Left menu → **Deployment Center**
3. Select **GitHub** → Authorize
4. Choose:
   - **Organization**: Your GitHub username/org
   - **Repository**: `c-shlopes`
   - **Branch**: `main`
   - **Build provider**: GitHub Actions (recommended)
5. Click "Save"
6. Azure will automatically create a GitHub Actions workflow

### 4. Update GitHub Actions Workflow

The auto-generated workflow should work, but if you need to customize it, the workflow file is at:
```
.github/workflows/main_ski-field-tracker-api.yml
```

### 5. Run Database Migrations and Seed

**Option A: Using Azure Cloud Shell**
1. In Azure Portal, click the Cloud Shell icon (top bar)
2. Run:
```bash
az webapp ssh --name ski-field-tracker-api --resource-group ski-field-tracker
cd /home/site/wwwroot
dotnet SkiFieldTracker.Api.dll -- --seed
```

**Option B: Using Kudu Console**
1. Go to `https://ski-field-tracker-api.scm.azurewebsites.net`
2. Click "Debug console" → "CMD"
3. Navigate to `site/wwwroot`
4. Run: `dotnet SkiFieldTracker.Api.dll -- --seed`

### 6. Get API URL

Your API URL will be:
```
https://ski-field-tracker-api.azurewebsites.net
```

## Free Tier (F1) Limits

- **60 minutes compute time/month**
- **1 GB storage**
- **5 GB bandwidth/month**

For production, upgrade to Basic B1 ($13/month) for unlimited compute time.

## Troubleshooting

- **Deployment failed**: Check GitHub Actions logs
- **API not accessible**: Check environment variables
- **Database connection failed**: Verify connection string format

## Manual Deployment (if needed)

```bash
# Install Azure CLI
# macOS: brew install azure-cli
az login

# Build and publish
cd backend/SkiFieldTracker.Api
dotnet publish -c Release -o ./publish

# Deploy
az webapp deploy --name ski-field-tracker-api --resource-group ski-field-tracker --src-path ./publish
```
