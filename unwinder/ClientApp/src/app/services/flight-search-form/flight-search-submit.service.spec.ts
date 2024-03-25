import { TestBed } from '@angular/core/testing';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { FlightSearchSubmitService } from './flight-search-submit.service';
import { FlightSearchData } from '../../interfaces/flight-data-exchange/flight-search-data';
import { HttpErrorResponse } from '@angular/common/http';

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

  it('should handle errors for submitFlightSearchDataToApi', () => {
    const mockData: FlightSearchData = {
      where: 'New York',
      origin: 'London',
      when: new Date('2022-01-01'),
      numberOfPassengers: 2,
    };
    const mockError = new Error('An error occurred');

    service.submitFlightSearchDataToApi(mockData).subscribe({
      next: () => fail('Should have failed with the 500 status'),
      error: (error: HttpErrorResponse) => {
        expect(error.status).toBe(500);
      },
    });

    const req = httpTestingController.expectOne(service.apiUrl);
    req.flush('An error occurred', {
      status: 500,
      statusText: 'Internal Server Error',
    });
  });
});
