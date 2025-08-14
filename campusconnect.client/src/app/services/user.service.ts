import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserEntity } from '../entities/userEntity';
import { baseApiRoute } from '../app-routing.module';
import { Observable } from 'rxjs';
import { LoginDto } from '../entities/loginDto';
import { LoginResponseDto } from '../entities/loginResponseDto';
import { Module } from '../entities/module';

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

    public checkIfLoginNameExists(loginName: string): Observable<boolean> {
        return this.httpClient.get<boolean>(baseApiRoute + "user/exists/" + loginName);
    }

    public deleteUser(userID: number): Observable<void> {
        return this.httpClient.delete<void>(baseApiRoute + "user/" + userID);
    }

    public getUserIdByLoginName(loginName: string): Observable<Number> {
        return this.httpClient.get<Number>(baseApiRoute + "user/getId/" + loginName);
    }

    public postUserModules(modules: Module[], id: Number): Observable<string[]> {
        return this.httpClient.post<string[]>(baseApiRoute + "user/saveModules/" + id, modules);
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
