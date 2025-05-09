import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthGuardTestComponent } from './auth-guard-test.component';

describe('AuthGuardTestComponent', () => {
  let component: AuthGuardTestComponent;
  let fixture: ComponentFixture<AuthGuardTestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AuthGuardTestComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AuthGuardTestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
