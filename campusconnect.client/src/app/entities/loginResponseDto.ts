import { UserRoleEntity } from "./userRoleEntity";

export class LoginResponseDto {
    constructor(
        token: string,
        username: string,
        role: UserRoleEntity,
        requiresTwoFactor?: boolean,
        tempToken?: string
    ) {
        this.role = role;
        this.username = username;
        this.token = token;
        this.requiresTwoFactor = requiresTwoFactor || false;
        this.tempToken = tempToken;
    }

    public token: string;
    public username: string;
    public role: UserRoleEntity;
    public requiresTwoFactor: boolean;
    public tempToken?: string;
}