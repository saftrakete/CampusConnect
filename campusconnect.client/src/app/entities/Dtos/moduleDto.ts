import { DifficultyEnum } from "../../enums/difficultyEnum";

export class ModuleDto {
    constructor(
        name: string, 
        facultyId: number, 
        difficulty: DifficultyEnum) {
            this.name = name;
            this.facultyId = facultyId;
            this.difficulty = difficulty;
    }

    public name: string;
    public facultyId: number;
    public difficulty: DifficultyEnum;
}