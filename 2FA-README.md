# Two-Factor Authentication Implementation

## Overview
This implementation adds TOTP (Time-based One-Time Password) 2FA to your CampusConnect application using ASP.NET Core backend and Angular frontend.

## Backend Changes
- Added `TwoFactorEnabled` and `TwoFactorSecret` fields to `UserModel`
- Created `TwoFactorService` for TOTP operations using OtpNet
- Added 2FA endpoints in `UserController`:
  - `POST /user/2fa/setup` - Generate QR code for 2FA setup
  - `POST /user/2fa/verify` - Verify setup code
  - `POST /user/2fa/login` - Verify login code
  - `POST /user/2fa/disable` - Disable 2FA
- Updated login flow to handle 2FA requirement

## Frontend Changes
- Created `TwoFactorComponent` for login verification
- Created `TwoFactorSetupComponent` for enabling/disabling 2FA
- Updated `LoginComponent` to handle 2FA flow
- Added routing for 2FA components

## Setup Instructions
1. Install required packages: `dotnet restore` in the Server project
2. Add 2FA fields to database:
   - Run the SQL script in `Migrations/Add2FAFields.sql`
   - Or use: `dotnet ef migrations add Add2FAFields && dotnet ef database update`
3. Build and run the application
4. Test 2FA setup at `/api/test/2fa-test` endpoint

## Usage
1. Users can enable 2FA by navigating to `/two-factor-setup`
2. Scan the QR code with an authenticator app (Google Authenticator, Authy, etc.)
3. Enter the 6-digit code to verify setup
4. On subsequent logins, users will be redirected to `/two-factor` to enter their code
5. Users can disable 2FA from the setup page

## Security Features
- Temporary tokens for 2FA login flow (5-minute expiry)
- TOTP codes with 30-second window
- Secure secret generation using cryptographic random
- JWT tokens include user ID for proper authorization