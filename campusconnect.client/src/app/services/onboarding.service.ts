import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { baseApiRoute } from '../app-routing.module';
import { Module } from '../entities/module';

@Injectable({
  providedIn: 'root'
})
export class OnboardingService {
  selectedSemester: Number | null = null;
  selectedStudy: string | null = null;

  filteredModules: Module[] = [];

  constructor(private httpClient: HttpClient) { }

  public printInfo() {
    console.log(this.selectedSemester + "  " + this.selectedStudy);
    this.httpClient.get<Module[]>(baseApiRoute + "degree/onboarding/" + this.selectedStudy).subscribe(
      (modules: Module[]) => {
        this.filteredModules = modules
        console.log(this.filteredModules.length)
        this.filteredModules.forEach(e => {
          console.log(e + " ==> " + e.moduleId + " | " + e.name);
        });
      }
    );
  }
}
