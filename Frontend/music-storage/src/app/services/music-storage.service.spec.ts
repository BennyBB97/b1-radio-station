import { TestBed } from '@angular/core/testing';

import { MusicStorageService } from './music-storage.service';

describe('MusicStorageService', () => {
  let service: MusicStorageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MusicStorageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
