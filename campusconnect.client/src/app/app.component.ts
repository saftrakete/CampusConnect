import { HttpClient } from '@angular/common/http';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { InitializeDbTablesService } from './services/initialize-db-tables.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  constructor(private http: HttpClient, private initDB: InitializeDbTablesService) {}

  ngOnInit(): void {
    this.initDB.clearModuleTable();
    this.initDB.fillModuleTable();
  }

  title = 'campusconnect.client';
}
