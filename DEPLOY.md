# Deployment Guide - Fly.io (Simplest, Free Tier, Node.js-like Experience)

## Why Fly.io?
- ✅ **Simplest .NET deployment** - Just 3 commands
- ✅ **Free tier** - 3 shared CPU VMs, 256MB RAM each
- ✅ **Vercel-like experience** - CLI deployment, auto HTTPS
- ✅ **Zero configuration** - Config files are ready

## Quick Deployment Steps (5 minutes)

### 1. Install Fly CLI

```bash
# macOS
curl -L https://fly.io/install.sh | sh

# Or using Homebrew
brew install flyctl
```

### 2. Login to Fly.io

```bash
flyctl auth login
```

### 3. Build and Deploy

```bash
cd backend/SkiFieldTracker.Api

# Publish .NET application
dotnet publish -c Release -o ./publish

# First-time deployment (will auto-create app)
flyctl launch --no-deploy

# Set environment variables
flyctl secrets set ConnectionStrings__DefaultConnection="Your Supabase connection string"
flyctl secrets set ASPNETCORE_ENVIRONMENT=Production

# Deploy
flyctl deploy
```

### 4. Run Database Migrations and Seed

```bash
# SSH into Fly.io machine
flyctl ssh console

# Run inside the machine
cd /app
dotnet SkiFieldTracker.Api.dll -- --seed
```

### 5. Get API URL

After deployment, your API URL will be:
```
https://ski-field-tracker-api.fly.dev
```

## Subsequent Deployments (Just 1 command)

```bash
cd backend/SkiFieldTracker.Api
dotnet publish -c Release -o ./publish
flyctl deploy
```

## Free Tier Limits

- **3 shared CPU VMs**
- **256MB RAM per VM**
- **160GB outbound traffic/month**
- **3GB persistent storage**

More than enough for an API!

## Troubleshooting

- **Deployment failed**: Run `flyctl logs` to view logs
- **API not accessible**: Check `flyctl status`
- **Database connection failed**: Verify environment variables are set correctly

## Update Environment Variables

```bash
flyctl secrets set ConnectionStrings__DefaultConnection="New connection string"
```

## View Logs

```bash
flyctl logs
```

## Stop/Start App (Save Resources)

```bash
flyctl scale count 0  # Stop
flyctl scale count 1  # Start
```
