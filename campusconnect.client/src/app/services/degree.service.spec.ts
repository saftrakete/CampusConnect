import { TestBed } from '@angular/core/testing';

import { DegreeService } from './degree.service';

describe('DegreeService', () => {
  let service: DegreeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DegreeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
