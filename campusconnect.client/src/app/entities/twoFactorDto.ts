export class TwoFactorSetupDto {
  constructor(
    public qrCodeUri: string,
    public manualEntryKey: string
  ) {}
}

export class TwoFactorVerifyDto {
  constructor(
    public LoginName: string,
    public Code: string,
    public TempToken?: string
  ) {}
}

export class TwoFactorLoginResponseDto {
  constructor(
    public requiresTwoFactor: boolean,
    public tempToken?: string,
    public token?: string,
    public username?: string
  ) {}
}