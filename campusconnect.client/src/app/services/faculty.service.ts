import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { FacultyEntity } from '../entities/facultyEntity';
import { baseApiRoute } from '../app-routing.module';
import { FacultyDto } from '../entities/Dtos/facultyDto';

@Injectable({
  providedIn: 'root'
})
export class FacultyService {

  constructor(private httpClient: HttpClient) { }

  public getFaculties(): Observable<FacultyEntity[]> {
    return this.httpClient.get<FacultyEntity[]>(baseApiRoute + "faculties/get/all");
  }

  public postNewFaculty(facultyDto: FacultyDto): Observable<FacultyEntity> {
    return this.httpClient.post<FacultyEntity>(baseApiRoute + "faculties/postFaculty", facultyDto);
  }

  public deleteFaculty(facultyId: number): Observable<Object> {
    return this.httpClient.delete(baseApiRoute + "faculties/delete/" + facultyId);
  }

  public createFacultyDto(name: string) {
    return new FacultyDto(name);
  }
}
