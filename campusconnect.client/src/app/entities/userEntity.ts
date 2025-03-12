export class UserEntity {
    constructor(
        nickname: string,
        loginName: string,
        password: string,
        userId?: number) {
        this.loginName = loginName;
        this.nickname = nickname;
        this.password = password;
        this.userId = userId;
    }

    public userId?: number;
    public nickname: string;
    public loginName: string;
    private password: string;
}