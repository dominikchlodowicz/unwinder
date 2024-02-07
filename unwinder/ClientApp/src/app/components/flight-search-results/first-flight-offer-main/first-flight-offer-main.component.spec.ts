import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { FirstFlightOfferMainComponent } from './first-flight-offer-main.component';
import { UnwinderSessionService } from '../../../services/unwinder-search-state/unwinder-session.service';
import { FlightSearchSubmitService } from '../../../services/flight-search-form/flight-search-submit.service';
import { of } from 'rxjs';
import { FlightSearchResponse } from '../../../interfaces/flight-data-exchange/flight-search-response';
import { Router } from '@angular/router';
import { FlightSearchData } from '../../../interfaces/flight-data-exchange/flight-search-data';
import { throwError } from 'rxjs';

describe('FirstFlightOfferMainComponent', () => {
  let component: FirstFlightOfferMainComponent;
  let fixture: ComponentFixture<FirstFlightOfferMainComponent>;
  let router: Router;
  let mockGetSessionDataObservable = jest.fn();
  let mockSubmitFlightSearchDataToApi = jest.fn();

  const mockFlightSearchResponse: FlightSearchResponse = {
    meta: { count: 1 },
    data: [
      {
        itineraries: [
          {
            duration: 'P1D',
            segments: [
              {
                departure: {
                  iataCode: 'LAX',
                  at: '2024-02-05T07:00:00',
                },
                arrival: {
                  iataCode: 'JFK',
                  at: '2024-02-05T14:00:00',
                },
                duration: 'PT5H',
                carrierCode: 'DL',
                number: '1234',
              },
            ],
          },
        ],
        price: {
          total: '450.00',
          currency: 'USD',
        },
      },
    ],
  };

  const mockFlightSearchData: FlightSearchData = {
    where: 'NYC',
    origin: 'LAX',
    when: new Date('2024-01-01'),
    numberOfPassengers: 2,
  };

  beforeEach(async () => {
    mockSubmitFlightSearchDataToApi.mockReturnValue(
      of(mockFlightSearchResponse),
    );

    mockGetSessionDataObservable.mockReturnValue(
      of({
        flightBackData: new Date(),
        firstFlightResponse: {},
        flightParameters: {},
      }),
    );

    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule.withRoutes([]),
        FirstFlightOfferMainComponent,
      ],
      providers: [
        {
          provide: UnwinderSessionService,
          useValue: {
            getSessionDataObservable: mockGetSessionDataObservable,
            setData: jest.fn(() => of(null)),
          },
        },
        {
          provide: FlightSearchSubmitService,
          useValue: {
            serializeFlightSearchData: jest.fn(),
            submitFlightSearchDataToApi: mockSubmitFlightSearchDataToApi,
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(FirstFlightOfferMainComponent);
    component = fixture.componentInstance;

    component.firstFlightResponse = mockFlightSearchResponse;
    component.flightParameters = mockFlightSearchData;
    router = TestBed.inject(Router);
  });

  it('should create', () => {
    expect(component).toBeDefined();
  });

  it('should navigate to "/unwind/second-flight" on successful flight selection', async () => {
    const navigateSpy = jest.spyOn(router, 'navigate');

    component.submitSelectedFlight(0);
    await fixture.whenStable();

    expect(navigateSpy).toHaveBeenCalledWith(['/unwind/second-flight']);
  });

  it('should handle errors during flight selection submission', async () => {
    const errorSpy = jest.spyOn(console, 'error');
    const mockError = new Error('API error');

    mockSubmitFlightSearchDataToApi.mockReturnValue(
      throwError(() => mockError),
    );

    component.submitSelectedFlight(0);

    await fixture.whenStable();

    expect(errorSpy).toHaveBeenCalledWith('Error updating data:', mockError);
  });

  it('should navigate correctly for different flight selections', async () => {
    const navigateSpy = jest.spyOn(router, 'navigate');

    component.submitSelectedFlight(1);
    await fixture.whenStable();
  });
});
