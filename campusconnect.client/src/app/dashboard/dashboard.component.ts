import { Component } from '@angular/core';
import { ModuleService } from '../services/module.service'; 

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
  constructor(public modService: ModuleService) {}
}
