import { DifficultyEnum } from "../enums/difficultyEnum";
import { FacultyEntity } from "./facultyEntity";

export class ModuleEntity {
    constructor(
        name: string, 
        faculty: FacultyEntity, 
        difficulty: DifficultyEnum, 
        moduleId?: number) {
        this.difficulty = difficulty;
        this.name = name;
        this.faculty = faculty;
        this.moduleId = moduleId;
    }

    public moduleId?: number;
    public name: string;
    public faculty: FacultyEntity;
    public difficulty: DifficultyEnum;
}