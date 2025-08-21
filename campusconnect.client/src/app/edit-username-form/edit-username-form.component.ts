import { Component } from '@angular/core';
import { UserService } from '../services/user.service';
import { AuthorizationService } from '../services/authorization.service';

@Component({
  selector: 'app-edit-username-form',
  templateUrl: './edit-username-form.component.html',
  styleUrl: './edit-username-form.component.scss'
})
export class EditUsernameFormComponent {

  constructor(public us: UserService, public auth: AuthorizationService) {}

  public confirmUsernameUpdate() {
    const loginName = this.auth.getLoginName();
    if (loginName) {
      // Erstellt neues Dto und sendet mittels UserService Anfrage ans Backend
      this.us.updateUsername(this.us.createChangeUsernameDto(loginName, this.us.newUsername)).subscribe();
    }
  }
}
