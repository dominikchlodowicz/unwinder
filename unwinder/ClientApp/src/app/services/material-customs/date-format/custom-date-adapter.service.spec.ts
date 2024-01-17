import { TestBed } from '@angular/core/testing';
import { CustomDateAdapterService } from './custom-date-adapter.service';
import {
  CUSTOM_EUROPE_DATE_FORMAT_SERVICE,
  MONDAY_CUSTOM_DATE_ADAPTER_SERVICE,
} from '../../../injection-tokens/material-injection-tokens';
import { MAT_DATE_LOCALE } from '@angular/material/core';

interface MockMondayCustomDateAdapterService {
  getFirstDayOfWeek: jest.Mock;
}

interface MockCustomEuropeDateFormatService {
  parse: jest.Mock;
}

describe('CustomDateAdapterService', () => {
  let service: CustomDateAdapterService;
  let mockMondayAdapter: MockMondayCustomDateAdapterService;
  let mockEuropeAdapter: MockCustomEuropeDateFormatService;
  beforeEach(() => {
    mockMondayAdapter = {
      getFirstDayOfWeek: jest.fn(),
    };

    mockEuropeAdapter = {
      parse: jest.fn(),
    };

    TestBed.configureTestingModule({
      providers: [
        CustomDateAdapterService,
        {
          provide: MONDAY_CUSTOM_DATE_ADAPTER_SERVICE,
          useValue: mockMondayAdapter,
        },
        {
          provide: CUSTOM_EUROPE_DATE_FORMAT_SERVICE,
          useValue: mockEuropeAdapter,
        },
        { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
      ],
    });

    service = TestBed.inject(CustomDateAdapterService);
  });

  it('getFirstDayOfWeek should delegate to mondayAdapter', () => {
    mockMondayAdapter.getFirstDayOfWeek.mockReturnValue(1); // Monday
    expect(service.getFirstDayOfWeek()).toBe(1);
    expect(mockMondayAdapter.getFirstDayOfWeek).toHaveBeenCalled();
  });

  it('parse should delegate to europeAdapter', () => {
    const mockDate = new Date();
    const dateString = '01/01/2022';

    mockEuropeAdapter.parse.mockReturnValue(mockDate);
    expect(service.parse(dateString, null)).toBe(mockDate);
    expect(mockEuropeAdapter.parse).toHaveBeenCalledWith(dateString);
  });
});
