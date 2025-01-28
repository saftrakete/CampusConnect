import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserEntity } from '../entities/userEntity';
import { baseApiRoute } from '../app-routing.module';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

    constructor(private httpClient: HttpClient) { }

    public postNewUser(user: UserEntity): Observable<UserEntity> {
        return this.httpClient.post<UserEntity>(baseApiRoute + "user", user);
    }

    public createUserEntity(
        nickname: string,
        loginName: string,
        password: string,
        userId?: number
    ): UserEntity {
        return new UserEntity(nickname, loginName, password, userId);
    }
}
