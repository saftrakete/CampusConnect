import { TestBed } from '@angular/core/testing';

import { OnboardingServiceService } from './onboarding-service.service';

describe('OnboardingServiceService', () => {
  let service: OnboardingServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OnboardingServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
