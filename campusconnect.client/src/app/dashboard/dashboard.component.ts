import { Component } from '@angular/core';
import { OnboardingService } from '../services/onboarding.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {

  filterQuery: string = '';

  constructor(public os: OnboardingService) {}

  public filter() {
    console.log(this.filterQuery);
  }

}
