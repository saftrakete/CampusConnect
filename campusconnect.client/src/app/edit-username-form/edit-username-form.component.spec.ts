import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditUsernameFormComponent } from './edit-username-form.component';

describe('EditUsernameFormComponent', () => {
  let component: EditUsernameFormComponent;
  let fixture: ComponentFixture<EditUsernameFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [EditUsernameFormComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EditUsernameFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
