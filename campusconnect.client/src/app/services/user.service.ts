import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserEntity } from '../entities/userEntity';
import { baseApiRoute } from '../app-routing.module';
import { Observable } from 'rxjs';
import { LoginDto } from '../entities/loginDto';

@Injectable({
  providedIn: 'root'
})
export class UserService {

    constructor(private httpClient: HttpClient) { }

    public postNewUser(user: UserEntity): Observable<UserEntity> {
        return this.httpClient.post<UserEntity>(baseApiRoute + "user", user);
    }

    public sendLoginRequest(loginDto: LoginDto): Observable<UserEntity> {
        return this.httpClient.post<UserEntity>(baseApiRoute + 'user/login', loginDto);
    }

    public checkIfLoginNameExists(loginName: string): Observable<boolean> {
        return this.httpClient.get<boolean>(baseApiRoute + "user/exists/" + loginName);
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
