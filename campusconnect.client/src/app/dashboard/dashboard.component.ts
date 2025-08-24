import { Component, OnInit } from '@angular/core';
import { OnboardingService } from '../services/onboarding.service';
import { AuthorizationService } from '../services/authorization.service';
import { UserService } from '../services/user.service';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {

  filterQuery: string = '';

  constructor(public os: OnboardingService, public auth: AuthorizationService, public us: UserService) {}

    ngOnInit(): void {
    this.os.addedModules = [];
    this.os.LoadOnboardingDataFromLocalStorage();
    this.os.SaveOnboardingDataToLocalStorage();
    this.os.StartOnboardingFiltering();
  }

  public userFilter() {
    this.os.filteredModules = this.os.filteredModules.filter(mod => mod.name.toLowerCase().includes(this.filterQuery.toLowerCase()));
  }

  public resetFiltering() {
    this.filterQuery = '';
    this.os.StartOnboardingFiltering();
  }

  // LastValueFrom wird genutzt damit CleanUp erst dann ausgeführt wird, wenn die Module fertig gespeichert wurden.
  // ==> Durch LastValueFrom lässt sich Await verwenden.
  public async confirmActions() {
    const loginName = this.auth.getLoginName();

    if (loginName != null) {
      try {
        const userId = await lastValueFrom(this.us.getUserIdByLoginName(loginName));
        const savedModules = await lastValueFrom(this.us.postUserModules(this.os.addedModules, userId));

        console.log('Saved modules:', savedModules);
        this.os.onboardingCompleted = true;

        this.os.CleanUp();
      } catch (err) {
        console.error('Error during onboarding:', err);
      }
    }
  }

}
