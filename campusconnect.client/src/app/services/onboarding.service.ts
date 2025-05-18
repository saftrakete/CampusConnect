import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { baseApiRoute } from '../app-routing.module';
import { Module } from '../entities/module';

@Injectable({
  providedIn: 'root'
})
export class OnboardingService {
  selectedSemester: number = 1;
  selectedStudy: string | null = null;

  filteredModules: Module[] = []
  addedModules: Module[] = [];

  constructor(private httpClient: HttpClient) { }

  public async StartOnboardingFiltering() {
    if (this.selectedStudy != null) {
      await this.httpClient.get<Module[]>(baseApiRoute + "degree/onboarding/" + this.selectedStudy).subscribe(    // Filtert Module nach Studiengang
        (modules: Module[]) => {
          this.filteredModules = modules;
          this.filteredModules = this.filteredModules.filter(mod => mod.semester <= this.selectedSemester);       // Filtert diese Module nochmals nach Semester (Man könnte auch '==' statt '<=' nehmen, wenn man nur Module aus dem einen Semester haben will)
        }                                                                                                                         
      );
    }
  }

  public AddModule(mod: Module) {
    this.addedModules.push(mod);
  }

  public DeleteModule(mod: Module) {
    const index = this.addedModules.findIndex(m => m.moduleId === mod.moduleId);
    this.addedModules.splice(index, 1);
  }

  public IsModuleAdded(mod: Module) {
    return this.addedModules.includes(mod);
  }
}
