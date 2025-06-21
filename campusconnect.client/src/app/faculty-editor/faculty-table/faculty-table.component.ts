import { Component, OnInit } from '@angular/core';
import { FacultyService } from '../../services/faculty.service';
import { FacultyEntity } from '../../entities/facultyEntity';

@Component({
  selector: 'faculty-table',
  templateUrl: './faculty-table.component.html',
  styleUrl: './faculty-table.component.scss'
})
export class FacultyTableComponent implements OnInit {
  public constructor(private facultyService: FacultyService) {
  }
  
  public ngOnInit(): void {
    this.facultyService.getFaculties().subscribe(response => {
      console.log(response);

      if (response !== undefined) {
        this.faculties = response;
      }
    });
  }

  public faculties!: FacultyEntity[];
}
