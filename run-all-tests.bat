@echo off
echo Running Complete Test Suite...
echo.

echo [1/3] Backend Unit Tests
cd CampusConnect.Server.Tests
dotnet test --verbosity normal --filter "FullyQualifiedName!~Integration"
if %ERRORLEVEL% NEQ 0 (
    echo ❌ Backend tests failed
    pause
    exit /b 1
)
echo ✅ Backend tests passed
echo.

echo [2/3] Frontend Tests
cd ..\campusconnect.client
call npm run test:ci
if %ERRORLEVEL% NEQ 0 (
    echo ❌ Frontend tests failed
    pause
    exit /b 1
)
echo ✅ Frontend tests passed
echo.

echo [3/3] E2E Tests
cd ..\CampusConnect.E2E.Tests
call npm test
if %ERRORLEVEL% NEQ 0 (
    echo ❌ E2E tests failed
    pause
    exit /b 1
)
echo ✅ E2E tests passed
echo.

echo 🎉 All tests passed successfully!
pause