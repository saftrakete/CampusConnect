export class UserRoleEntity {
    constructor(
        roleName: string,
        roleDescription: string,
        permissions: string[],
        userRoleId?: number
    ) {
        this.permissions = permissions;
        this.roleDescription = roleDescription;
        this.roleName = roleName;
        this.userRoleId = userRoleId;
    }

    public userRoleId?: number;
    public roleName: string;
    public roleDescription: string;
    public permissions: string[];
}