import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { baseApiRoute } from '../app-routing.module';
import { TwoFactorSetupDto, TwoFactorVerifyDto } from '../entities/twoFactorDto';

@Injectable({
  providedIn: 'root'
})
export class TwoFactorService {
  constructor(private httpClient: HttpClient) {}

  public setupTwoFactor(loginName: string): Observable<TwoFactorSetupDto> {
    return this.httpClient.post<TwoFactorSetupDto>(baseApiRoute + 'user/2fa/setup', { LoginName: loginName });
  }

  public verifyTwoFactor(verifyDto: TwoFactorVerifyDto): Observable<string> {
    return this.httpClient.post<string>(baseApiRoute + 'user/2fa/verify', verifyDto);
  }

  public verifyTwoFactorSetup(loginName: string, code: string): Observable<string> {
    return this.httpClient.post<string>(baseApiRoute + 'user/2fa/verify', { LoginName: loginName, Code: code });
  }

  public disableTwoFactor(code: string): Observable<void> {
    return this.httpClient.post<void>(baseApiRoute + 'user/2fa/disable', { code });
  }
}