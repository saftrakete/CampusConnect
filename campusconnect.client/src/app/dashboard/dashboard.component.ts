import { Component, OnInit } from '@angular/core';
import { OnboardingService } from '../services/onboarding.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {

  filterQuery: string = '';

  constructor(public os: OnboardingService) {}

    ngOnInit(): void {
    this.os.loadOnboardingDataFromLocalStorage();
    this.os.saveOnboardingDataToLocalStorage();
    this.os.StartOnboardingFiltering();
  }

  public userFilter() {
    this.os.filteredModules = this.os.filteredModules.filter(mod => mod.name.toLowerCase().includes(this.filterQuery.toLowerCase()));
  }

  public resetFiltering() {
    this.filterQuery = '';
    this.os.StartOnboardingFiltering();
  }

}
