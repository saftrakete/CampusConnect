import { Component } from '@angular/core';
import { OnboardingService } from '../services/onboarding.service';

@Component({
  selector: 'app-onboarding',
  templateUrl: './onboarding.component.html',
  styleUrl: './onboarding.component.scss'
})
export class OnboardingComponent {

  constructor(public os: OnboardingService) {} 

  protected semesters: Number[] = [1,2,3,4,5,6,7,8,9,10];
  protected studyPrograms: string[] = [
    'Informatik', 
    'Ingenieurinformatik'
  ];
}
