export class DegreeDto {
    public constructor(
        name: string,
        facultyId: number,
        mandatoryModuleIds: number[]
    ) {
        this.name = name;
        this.facultyId = facultyId;
        this.mandatoryModuleIds = mandatoryModuleIds;
    }

    public name: string;
    public facultyId: number;
    public mandatoryModuleIds: number[];
}