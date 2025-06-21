import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FacultyEditorComponent } from './faculty-editor.component';

describe('FacultyEditorComponent', () => {
  let component: FacultyEditorComponent;
  let fixture: ComponentFixture<FacultyEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FacultyEditorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(FacultyEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
