import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { FacultyService } from '../../services/faculty.service';

@Component({
  selector: 'add-faculty-dialog',
  templateUrl: './add-faculty-dialog.component.html',
  styleUrl: './add-faculty-dialog.component.scss'
})
export class AddFacultyDialogComponent implements OnInit {
  public constructor(
    public dialogRef: MatDialogRef<AddFacultyDialogComponent>,
    private formBuilder: FormBuilder,
    private facultyService: FacultyService
  ) {
  }

  ngOnInit(): void {
    this.facultyForm = this.formBuilder.group({
      facultyName: [this.emptyString, Validators.required]
    });
  }

  public facultyForm!: FormGroup
  private readonly emptyString: string = '';

  public onDoneClick(): void {
    if (!this.facultyForm.valid) {
      console.warn("Faculty form is invalid");
      return;
    }

    let facultyName = this.facultyForm.value.facultyName;
    let facultyDto = this.facultyService.createFacultyDto(facultyName);
    this.dialogRef.close(facultyDto);
  }

  public onCancelClick(): void {
    this.dialogRef.close();
  }
}
