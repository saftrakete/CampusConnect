import { TestBed } from '@angular/core/testing';

import { InitializeDbTablesService } from './initialize-db-tables.service';

describe('InitializeDbTablesService', () => {
  let service: InitializeDbTablesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(InitializeDbTablesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
