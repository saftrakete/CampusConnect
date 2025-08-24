# GitHub Actions Workflows

## Available Workflows

### 1. CI/CD Pipeline (`ci.yml`)
**Triggers**: Push to main/develop, Pull Requests to main/develop
- ✅ Backend unit tests
- ✅ Frontend tests  
- ✅ E2E tests
- 📊 Test artifacts upload

### 2. Tests Only (`test-only.yml`)
**Triggers**: Manual dispatch, Daily at 2 AM
- ✅ Quick unit test run
- 🔄 Scheduled testing

### 3. PR Checks (`pr-checks.yml`)
**Triggers**: Pull Requests
- ✅ Build validation
- ✅ Unit tests
- ⚡ Fast feedback for PRs

### 4. Deploy (`deploy.yml`)
**Triggers**: Push to main/develop, Manual dispatch
- ✅ Test validation
- 🏗️ Production builds
- 🚀 Deployment ready

## Workflow Status
- **Unit Tests**: 13 tests
- **E2E Tests**: 15 tests
- **Coverage**: Backend + Frontend + User flows
- **Browsers**: Chrome (headless in CI)

## Manual Triggers
Go to Actions tab → Select workflow → "Run workflow"