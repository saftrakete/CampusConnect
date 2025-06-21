export class RoleDto {
    constructor(roleName: string,
        rolePermissions: string[],
        roleDescription: string
    ) {
        this.roleName = roleName;
        this.rolePermissions = rolePermissions;
        this.roleDescription = roleDescription;
    }

    public roleName: string;
    public rolePermissions: string[];
    public roleDescription: string;
}