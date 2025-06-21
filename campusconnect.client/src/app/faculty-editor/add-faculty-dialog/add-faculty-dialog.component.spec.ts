import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddFacultyDialogComponent } from './add-faculty-dialog.component';

describe('AddFacultyDialogComponent', () => {
  let component: AddFacultyDialogComponent;
  let fixture: ComponentFixture<AddFacultyDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddFacultyDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddFacultyDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
