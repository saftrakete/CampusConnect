@echo off
echo Running backend unit tests...
cd CampusConnect.Server.Tests
dotnet test --verbosity normal
pause