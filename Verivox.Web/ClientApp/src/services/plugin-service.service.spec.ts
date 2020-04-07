import { TestBed } from '@angular/core/testing';

import { PluginServiceService } from './plugin-service.service';

describe('PluginServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PluginServiceService = TestBed.get(PluginServiceService);
    expect(service).toBeTruthy();
  });
});
