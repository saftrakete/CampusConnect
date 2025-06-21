import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ModuleEntity } from '../entities/moduleEntity';
import { baseApiRoute } from '../app-routing.module';
import { ModuleDto } from '../entities/Dtos/moduleDto';
import { DifficultyEnum } from '../enums/difficultyEnum';

@Injectable({
  providedIn: 'root'
})
export class ModuleService {

  constructor(private httpClient: HttpClient) { }

  public getAllModules(): Observable<ModuleEntity[]> {
    return this.httpClient.get<ModuleEntity[]>(baseApiRoute + "modules/get/all");
  }

  public postModule(moduleDto: ModuleDto): Observable<ModuleEntity> {
    return this.httpClient.post<ModuleEntity>(baseApiRoute + "modules/postModule", moduleDto);
  }

  public deleteModule(moduleId: number): Observable<Object> {
    return this.httpClient.delete(baseApiRoute + "modules/delete/" + moduleId);
  }

  public createModuleDto(name: string, facultyId: number, difficulty: DifficultyEnum) {
    return new ModuleDto(name, facultyId, difficulty);
  }
}
