import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DegreeEntity } from '../entities/degreeEntity';
import { baseApiRoute } from '../app-routing.module';
import { DegreeDto } from '../entities/Dtos/degreeDto';

@Injectable({
  providedIn: 'root'
})
export class DegreeService {

  constructor(private httpClient: HttpClient) { }

  public getAllDegrees(): Observable<DegreeEntity[]> {
    return this.httpClient.get<DegreeEntity[]>(baseApiRoute + "degrees/get/all");
  }

  public postNewDegree(degreeDto: DegreeDto): Observable<DegreeEntity> {
    return this.httpClient.post<DegreeEntity>(baseApiRoute + "degrees/postDegree", degreeDto);
  }

  public deleteDegree(degreeId: number): Observable<Object> {
    return this.httpClient.delete(baseApiRoute + "degrees/delete/" + degreeId);
  }

  public createDegreeDto(name: string, facultyId: number, mandatoryModuleIds: number[]): DegreeDto {
    return new DegreeDto(name, facultyId, mandatoryModuleIds);
  }
}
