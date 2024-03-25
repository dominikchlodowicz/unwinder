import { TestBed } from '@angular/core/testing';
import { HotelSearchSubmitService } from './hotel-search-submit.service';
import { HotelSearchData } from '../../interfaces/hotel-data-exchange/hotel-search-data';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { HttpErrorResponse } from '@angular/common/http';

describe('HotelSearchSubmitService', () => {
  const mockData: HotelSearchData = {
    adults: 2,
    checkIn: new Date('2024-04-04'),
    checkOut: new Date('2024-04-06'),
    cityCode: 'NYC',
  };

  let service: HotelSearchSubmitService;
  let httpTestingController: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [HotelSearchSubmitService],
    });

    service = TestBed.inject(HotelSearchSubmitService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should serialize hotel search data correctly', () => {
    const result = service.serializeHotelSearchData(
      2,
      new Date('2024-04-04'),
      new Date('2024-04-06'),
      'NYC',
    );
    expect(result).toEqual(mockData);
  });

  it('should submit hotel search data to API and return result', (done) => {
    service.submitHotelSearchDataToApi(mockData).subscribe((data) => {
      expect(data).toEqual(mockData);
      done();
    });

    const req = httpTestingController.expectOne(service.apiUrl);
    expect(req.request.method).toBe('POST');
    req.flush(mockData);
  });

  it('should handle errors for submitHotelSearchDataToApi', () => {
    const mockError = new Error('An error occurred');

    service.submitHotelSearchDataToApi(mockData).subscribe({
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
