import { TestBed } from '@angular/core/testing';
import { HotelSearchSubmitService } from './hotel-search-submit.service';
import { HotelSearchData } from '../../interfaces/hotel-data-exchange/hotel-search-data';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { HttpErrorResponse, HttpParams } from '@angular/common/http';

describe('HotelSearchSubmitService', () => {
  const mockData: HotelSearchData = {
    adults: 2,
    checkIn: '2024-04-04',
    checkOut: '2024-04-06',
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

    const req = httpTestingController.expectOne((request) =>
      request.url.includes(service.apiUrl),
    );
    expect(req.request.method).toBe('GET');
    req.flush(mockData);
  });

  it('should handle errors for submitHotelSearchDataToApi', () => {
    const mockErrorMsg = 'An error occurred';
    const mockData: HotelSearchData = {
      adults: 2,
      checkIn: new Date('2024-04-04').toISOString().split('T')[0],
      checkOut: new Date('2024-04-06').toISOString().split('T')[0],
      cityCode: 'NYC',
    };

    service.submitHotelSearchDataToApi(mockData).subscribe({
      next: () => fail('Should have failed with the 500 status'),
      error: (error: HttpErrorResponse) => {
        expect(error.error).toBe(mockErrorMsg);
        expect(error.status).toBe(500);
      },
    });

    const params = new HttpParams()
      .set('adults', mockData.adults.toString())
      .set('checkIn', mockData.checkIn)
      .set('checkOut', mockData.checkOut)
      .set('cityCode', mockData.cityCode);

    const req = httpTestingController.expectOne(
      (req) =>
        req.method === 'GET' &&
        req.url === service.apiUrl &&
        req.params.toString() === params.toString(),
    );

    req.flush(mockErrorMsg, {
      status: 500,
      statusText: 'Internal Server Error',
    });
  });
});
