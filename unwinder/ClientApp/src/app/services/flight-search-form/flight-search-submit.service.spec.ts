import { TestBed } from '@angular/core/testing';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { FlightSearchSubmitService } from './flight-search-submit.service';
import { FlightSearchData } from '../../interfaces/flight-search-data';

describe('FlightSearchSubmitService', () => {
  let service: FlightSearchSubmitService;
  let httpTestingController: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [FlightSearchSubmitService],
    });

    service = TestBed.inject(FlightSearchSubmitService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should serialize flight search data correctly', () => {
    const mockData: FlightSearchData = {
      where: 'New York',
      origin: 'London',
      when: new Date('2022-01-01'),
      numberOfPassengers: 2,
    };

    const result = service.serializeFlightSearchData(
      'New York',
      'London',
      new Date('2022-01-01'),
      new Date('2022-01-07'),
      2,
    );
    expect(result).toEqual(mockData);
  });

  it('should submit flight search data to API and return result', (done) => {
    const mockData: FlightSearchData = {
      where: 'New York',
      origin: 'London',
      when: new Date('2022-01-01'),
      numberOfPassengers: 2,
    };

    service.submitFlightSearchDataToApi(mockData).subscribe((data) => {
      expect(data).toEqual(mockData);
      done();
    });

    const req = httpTestingController.expectOne(service.apiUrl);
    expect(req.request.method).toBe('POST');
    req.flush(mockData);
  });
});
