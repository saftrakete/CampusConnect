import { Component } from '@angular/core';
import { FacultyService } from '../services/faculty.service';
import { MatDialog } from '@angular/material/dialog';
import { AddFacultyDialogComponent } from './add-faculty-dialog/add-faculty-dialog.component';

@Component({
  selector: 'faculty-editor',
  templateUrl: './faculty-editor.component.html',
  styleUrl: './faculty-editor.component.scss'
})
export class FacultyEditorComponent {
  public constructor(
    private facultyService: FacultyService,
    private dialog: MatDialog
  ) {
  }

  public addFacultyClick(): void {
    const dialogRef = this.dialog.open(AddFacultyDialogComponent);
    dialogRef.afterClosed().subscribe(facultyDto => {
      if (facultyDto !== undefined) {
        console.log("adding faculty");
        console.log(facultyDto);

        this.facultyService.postNewFaculty(facultyDto).subscribe(response => {
          console.log(response);
        });
      }
    });
  }
}
