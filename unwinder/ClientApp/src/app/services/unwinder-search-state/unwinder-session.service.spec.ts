import { TestBed } from '@angular/core/testing';

import { UnwinderSessionService } from './unwinder-session.service';

describe('UnwinderSessionService', () => {
  let service: UnwinderSessionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UnwinderSessionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
