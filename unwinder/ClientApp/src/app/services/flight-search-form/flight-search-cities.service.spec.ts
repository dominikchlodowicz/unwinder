import { TestBed } from '@angular/core/testing';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { FlightSearchCitiesService } from './flight-search-cities.service';

describe('FlightSearchCitiesService', () => {
  let service: FlightSearchCitiesService;
  let httpTestingController: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [FlightSearchCitiesService],
    });

    service = TestBed.inject(FlightSearchCitiesService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('getCities should return expected cities (HttpClient called once)', (done) => {
    const mockCities = ['New York', 'Los Angeles', 'Chicago'];

    service.getCities('New').subscribe((cities) => {
      expect(cities).toEqual(mockCities);
      done();
    });

    const req = httpTestingController.expectOne(
      `${service.apiUrl}/get-city/New`,
    );
    expect(req.request.method).toBe('GET');
    req.flush(mockCities);
  });

  it('getCities should return expected cities (HttpClient called once)', (done) => {
    const mockCities = ['New York', 'Los Angeles', 'Chicago'];

    service.getCities('New').subscribe((cities) => {
      expect(cities).toEqual(mockCities);
      done();
    });

    const req = httpTestingController.expectOne(
      `${service.apiUrl}/get-city/New`,
    );
    expect(req.request.method).toBe('GET');
    req.flush(mockCities);
  });
});
