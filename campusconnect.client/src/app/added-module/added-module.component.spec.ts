import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddedModuleComponent } from './added-module.component';

describe('AddedModuleComponent', () => {
  let component: AddedModuleComponent;
  let fixture: ComponentFixture<AddedModuleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddedModuleComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddedModuleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
