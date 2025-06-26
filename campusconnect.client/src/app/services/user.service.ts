import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserEntity } from '../entities/userEntity';
import { baseApiRoute } from '../app-routing.module';
import { Observable } from 'rxjs';
import { LoginDto } from '../entities/loginDto';
import { LoginResponseDto } from '../entities/loginResponseDto';

@Injectable({
  providedIn: 'root'
})
export class UserService {

    constructor(private httpClient: HttpClient) { }

    public postNewUser(user: UserEntity): Observable<UserEntity> {
        return this.httpClient.post<UserEntity>(baseApiRoute + "user/register", user);
    }

    public sendLoginRequest(loginDto: LoginDto): Observable<LoginResponseDto> {
        return this.httpClient.post<LoginResponseDto>(baseApiRoute + 'user/login', loginDto);
    }

    public verifyTwoFactor(loginName: string, code: string, tempToken: string): Observable<LoginResponseDto> {
        return this.httpClient.post<LoginResponseDto>(baseApiRoute + 'user/2fa/login', {
            loginName,
            code,
            tempToken
        });
    }

    public checkIfLoginNameExists(loginName: string): Observable<boolean> {
        return this.httpClient.get<boolean>(baseApiRoute + "user/exists/" + loginName);
    }

    public deleteUser(userID: number): Observable<void> {
        return this.httpClient.delete<void>(baseApiRoute + "user/" + userID);
    }

    public createUserEntity(
        nickname: string,
        loginName: string,
        password: string,
        userId?: number
    ): UserEntity {
        return new UserEntity(nickname, loginName, password, userId);
    }

    public createLoginDto(
        loginName: string,
        password: string
    ): LoginDto {
        return new LoginDto(loginName, password);
    }

}
