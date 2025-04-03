import { Component } from '@angular/core';
import { ModuleService } from '../services/module.service'; 

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {

  filterQuery: string = '';

  constructor(public modService: ModuleService) {}

  public filter() {
    console.log(this.filterQuery);
  }

}
