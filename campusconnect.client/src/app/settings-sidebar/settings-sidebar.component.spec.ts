import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountSettingsSidebarComponent } from './settings-sidebar.component';

describe('AccountSettingsSidebarComponent', () => {
  let component: AccountSettingsSidebarComponent;
  let fixture: ComponentFixture<AccountSettingsSidebarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AccountSettingsSidebarComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AccountSettingsSidebarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
