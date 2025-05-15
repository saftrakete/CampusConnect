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

  filteredModules: Module[] = [];

  constructor(private httpClient: HttpClient) { }

  public async StartOnboardingFiltering() {
    if (this.selectedSemester != null && this.selectedStudy != null) {
      await this.httpClient.get<Module[]>(baseApiRoute + "degree/onboarding/" + this.selectedStudy).subscribe(
        (modules: Module[]) => {
          this.filteredModules = modules;
          console.log("After loading in: " + this.filteredModules.length);

          this.filteredModules = this.filteredModules.filter(mod => mod.semester <= this.selectedSemester);
          console.log("After filter: " + this.filteredModules.length);
        }
      );
    }
  }

  public test() {
    console.log("Test: " + this.filteredModules.length);
  }
}
