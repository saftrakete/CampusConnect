import { UserRoleEntity } from "./userRoleEntity";

export class UserEntity {
    constructor(
        nickname: string,
        loginName: string,
        password: string,
        userId?: number,
        role?: string) {
        this.loginName = loginName;
        this.nickname = nickname;
        this.password = password;
        this.userId = userId;
        this.role = role;
    }

    public userId?: number;
    public nickname: string;
    public loginName: string;
    public password: string;
    //public role?: UserRoleEntity;
    public role?: string;
}