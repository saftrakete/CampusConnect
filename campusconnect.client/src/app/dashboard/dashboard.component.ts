import { Component, OnInit } from '@angular/core';
import { OnboardingService } from '../services/onboarding.service';
import { AuthorizationService } from '../services/authorization.service';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {

  filterQuery: string = '';

  constructor(public os: OnboardingService, public auth: AuthorizationService, public us: UserService) {}

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

  public confirmActions() {
    const token = localStorage.getItem("jwt");
    const loginName = this.auth.getLoginName();
    if (loginName != null) {
      this.us.getUserIdByLoginName(loginName).subscribe(
        (userId) => {
          console.log(userId);
          // Id nutzen um addedModules als Liste in der User Tabelle für den individuellen Nutzer abzuspeichern
          //
          // TODO: UserModel anpassen, dass Listen gespeichert werden können. Anfrage ans Backend senden und Liste hinterlegen.
        }
      )
    }

  }

}
