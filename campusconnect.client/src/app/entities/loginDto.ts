export class LoginDto {
    constructor(
        loginName: string,
        password: string,
        role?: string
    ) {
        this.loginName = loginName;
        this.password = password;
        this.role = role;
    }

    public loginName: string;
    public password: string;
    public role?: string;
}