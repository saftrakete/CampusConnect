import { Component } from '@angular/core';
import { OnboardingServiceService } from '../services/onboarding-service.service';

@Component({
  selector: 'app-onboarding',
  templateUrl: './onboarding.component.html',
  styleUrl: './onboarding.component.scss'
})
export class OnboardingComponent {

  constructor(public os: OnboardingServiceService) {}  // Sorry für den Service-Namen

  semesters: Number[] = [1,2,3,4,5,6,7,8,9,10];
  studyPrograms: string[] = [
    'Informatik', 
    'Ingenieurinformatik'
  ];
}
