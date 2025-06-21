import { FacultyEntity } from "./facultyEntity";
import { ModuleEntity } from "./moduleEntity";

export class DegreeEntity {
    constructor(name: string,
        faculty: FacultyEntity,
        mandatoryModules: ModuleEntity[],
        degreeId?: number
    ) {
        this.name = name;
        this.faculty = faculty;
        this.mandatoryModules = mandatoryModules;
        this.degreeId = degreeId;
    }

    public degreeId?: number;
    public name: string;
    public faculty: FacultyEntity;
    public mandatoryModules: ModuleEntity[];
}