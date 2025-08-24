# CampusConnect E2E Tests

End-to-end tests for CampusConnect using Playwright.

## Setup

```bash
npm install
npx playwright install
```

## Running Tests

```bash
# Run all tests
npm test

# Run tests with browser visible
npm run test:headed

# Run tests with UI mode
npm run test:ui

# Show test report
npm run report
```

## Test Coverage

- **Authentication**: Login, registration, validation
- **Navigation**: Routing, 404 handling
- **Two-Factor Auth**: Setup and verification flows
- **User Settings**: Account management, settings navigation

## Prerequisites

- Angular frontend running on https://localhost:4200
- Backend API running and accessible