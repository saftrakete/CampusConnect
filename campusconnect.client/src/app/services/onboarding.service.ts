import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { baseApiRoute } from '../app-routing.module';
import { Module } from '../entities/module';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OnboardingService {
  selectedSemester: number = 1;
  selectedStudy: string | null = null;

  filteredModules: Module[] = []
  addedModules: Module[] = [];

  onboardingCompleted: boolean = false;    

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

  // Checkt ob Onboarding bereits abgeschlossen wurde
  public async CheckOnboardingStatus(loginName: string) {
    const status = await lastValueFrom(
      this.httpClient.get<boolean>(baseApiRoute + "user/check-onboarding-status/" + loginName)
    );
    this.onboardingCompleted = status;
    console.log(this.onboardingCompleted);
  }

  public GetOnboardingStatus() {
    return this.onboardingCompleted;
  }

  public CleanUp() {
    this.addedModules = [];
    this.CleanOnboardingDataFromLocalStorage();
  }

  public AddModule(mod: Module) {
    this.addedModules.push(mod);
  }

  public DeleteModule(mod: Module) {
    const index = this.addedModules.findIndex(m => m.moduleId === mod.moduleId);
    this.addedModules.splice(index, 1);
  }

  public IsModuleAdded(mod: Module) {
    return this.addedModules.some(m => m.name == mod.name);
  }

  public SaveOnboardingDataToLocalStorage() {
    localStorage.setItem('semester', this.selectedSemester.toString());
    localStorage.setItem('study', this.selectedStudy || '');
  }

  public LoadOnboardingDataFromLocalStorage() {
    const storedSemester = localStorage.getItem('semester');
    const storedStudy = localStorage.getItem('study');

    if (storedSemester) {
      this.selectedSemester = Number(storedSemester);
    }
    if (storedStudy) {
      this.selectedStudy = storedStudy || null;
    }
  }

  public CleanOnboardingDataFromLocalStorage() {
    localStorage.removeItem('semester');
    localStorage.removeItem('study');
  }
}
