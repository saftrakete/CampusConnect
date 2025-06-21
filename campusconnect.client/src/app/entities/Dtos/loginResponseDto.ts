import { UserRoleEntity } from "../userRoleEntity";

export class LoginResponseDto {
    constructor(
        token: string,
        username: string,
        role: UserRoleEntity
    ) {
        this.role = role;
        this.username = username;
        this.token = token;
    }

    public token: string;
    public username: string;
    public role: UserRoleEntity;
}