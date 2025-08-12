import { Component, OnInit } from '@angular/core';
import { AuthorizationService } from '../services/authorization.service';
import { OnboardingService } from '../services/onboarding.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  constructor(public auth: AuthorizationService, public os: OnboardingService) {}

  ngOnInit(): void {

    // Checkt einmal beim Initiieren der Homepage Komponente
    this.GetOnboardingStatus();
  }

  public async GetOnboardingStatus() {
    const loginName = this.auth.getLoginName();
    if (loginName) {
      this.os.CheckOnboardingStatus(loginName);
    } else {
      console.log("Nicht angemeldet!")
    }
  }
}
