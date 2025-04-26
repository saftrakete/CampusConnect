import { Injectable } from '@angular/core';
import { Module } from '../entities/module';
import { baseApiRoute } from '../app-routing.module';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class InitializeDbTablesService {

  modules: Module[] = [
    { "name": 'Mathe 1', "ID": 0, "facultyID": 0},
    { "name": 'Mathe 2', "ID": 1, "facultyID": 0},
    { "name": 'Mathe 3', "ID": 2, "facultyID": 0},
    { "name": 'Einführung Informatik', "ID": 3, "facultyID": 0},
    { "name": 'Technische Informatik 1', "ID": 4, "facultyID": 0},
    { "name": 'Technische Informatik 2', "ID": 5, "facultyID": 0},
    { "name": 'Datenbanken 1', "ID": 6, "facultyID": 0},
    { "name": 'Theoretische Informatik 1', "ID": 7, "facultyID": 0},
    { "name": 'Theoretische Informatik 2', "ID": 8, "facultyID": 0},
    { "name": 'Spezifikationstechnik', "ID": 9, "facultyID": 0},
    { "name": 'Programmierparadigmen', "ID": 10, "facultyID": 0},
    { "name": 'Introduction to Simulation', "ID": 11, "facultyID": 0}
  ];

  constructor(private httpClient: HttpClient) { }

  public async fillModuleTable() {
    var cnt = 0;
    for (const mod of this.modules) {
      try {
        await this.httpClient.post<Module>(baseApiRoute + "database/module", mod).toPromise();
        console.log(`Modul hinzugefügt: ${mod.name}`);
      } catch (error) {
        console.error(`Fehler beim Hinzufügen des Moduls ${mod.name}:`, error);
      }
    }
  }

  public clearModuleTable() {
    this.httpClient.delete<void>(baseApiRoute + "database/cleanTable").subscribe();
  }

}
