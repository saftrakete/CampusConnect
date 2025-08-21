import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserEntity } from '../entities/userEntity';
import { baseApiRoute } from '../app-routing.module';
import { Observable } from 'rxjs';
import { LoginDto } from '../entities/loginDto';
import { LoginResponseDto } from '../entities/loginResponseDto';
import { ChangeUsernameDto } from '../entities/changeUsernameDto';

@Injectable({
  providedIn: 'root'
})
export class UserService {

    newUsername: string = "";

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

    public getUserById(userId: number): Observable<UserEntity> {
        return this.httpClient.get<UserEntity>(`${baseApiRoute}user/${userId}`);
    }

    public getUsername(loginName: string): Observable<{ username: string }> {
        return this.httpClient.get<{ username: string }>(baseApiRoute + "user/getusername/" + loginName);
    }

    public updateUsername(changeUsernameDto: ChangeUsernameDto): Observable<void> {
        console.log(changeUsernameDto.loginName + " | " + changeUsernameDto.newNickname);
        return this.httpClient.post<void>(baseApiRoute + "user/updateNickname", changeUsernameDto);
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

    public createChangeUsernameDto(loginName: string, newNickname: string) {
        return new ChangeUsernameDto(loginName, newNickname);
    }

}
