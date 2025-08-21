export class ChangeUsernameDto {
    constructor(
        loginName: string,
        newNickname: string,
    ) {
        this.loginName = loginName;
        this.newNickname = newNickname;
    }

    public loginName: string;
    public newNickname: string;
}