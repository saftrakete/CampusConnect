import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class OnboardingServiceService {
  selectedSemester: Number | null = null;
  selectedStudy: string | null = null;

  constructor() { }

  public lol() {
    console.log(this.selectedSemester + "  " + this.selectedStudy);
  }
}
