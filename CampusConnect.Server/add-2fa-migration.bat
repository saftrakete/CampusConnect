@echo off
echo Adding 2FA migration...
dotnet ef migrations add Add2FAFields
echo Migration added successfully!
pause