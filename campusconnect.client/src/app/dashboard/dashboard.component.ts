import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashBoardComponent {
    userID = localStorage.getItem('token');
    message: string = 'Du bist User mit ID '+this.userID;
}
