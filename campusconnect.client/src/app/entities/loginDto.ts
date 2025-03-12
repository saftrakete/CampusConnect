export class LoginDto {
    constructor(
        loginName: string,
        password: string
    ) {
        this.loginName = loginName;
        this.password = password;
    }

    public loginName: string;
    public password: string;
}