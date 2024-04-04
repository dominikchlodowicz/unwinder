import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SecondFlightOfferMainComponent } from './second-flight-offer-main.component';
import { UnwinderSessionService } from '../../../services/unwinder-search-state/unwinder-session.service';
import { of } from 'rxjs';
import { FlightSearchData } from '../../../interfaces/flight-data-exchange/flight-search-data';
import { FlightSearchResponse } from '../../../interfaces/flight-data-exchange/flight-search-response';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';

describe('SecondFlightOfferMainComponent', () => {
  let component: SecondFlightOfferMainComponent;
  let fixture: ComponentFixture<SecondFlightOfferMainComponent>;
  let mockUnwinderSessionService = {
    getSessionDataObservable: jest.fn(),
    setData: jest.fn(),
    getData: jest.fn(),
  };

  const mockFlightSearchData: FlightSearchData = {
    where: 'JFK',
    origin: 'LAX',
    when: new Date('2024-02-05'),
    numberOfPassengers: 2,
  };

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

  beforeEach(async () => {
    mockUnwinderSessionService.getSessionDataObservable.mockReturnValue(
      of({
        secondFlightResponse: mockFlightSearchResponse,
      }),
    );

    mockUnwinderSessionService.getData.mockImplementation((key) => {
      if (key === 'flightParameters') {
        return {
          numberOfPassengers: 2,
          when: new Date('2024-02-05'),
          where: 'JFK',
        };
      }
      if (key === 'flightBackDate') {
        return new Date('2024-02-07');
      }

      return null;
    });

    await TestBed.configureTestingModule({
      imports: [
        SecondFlightOfferMainComponent,
        HttpClientTestingModule,
        RouterTestingModule,
      ],
      providers: [
        {
          provide: UnwinderSessionService,
          useValue: mockUnwinderSessionService,
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(SecondFlightOfferMainComponent);
    component = fixture.componentInstance;
  });

  it('should initialize secondFlightResponse from session data on ngOnInit', () => {
    const mockResponse = {
      data: [{}],
    };
    mockUnwinderSessionService.getSessionDataObservable.mockReturnValue(
      of({ secondFlightResponse: mockResponse }),
    );

    component.ngOnInit();

    expect(component.secondFlightResponse).toEqual(mockResponse);
  });

  it('should correctly set chosen flight data on submit', () => {
    const mockIndex = 0;
    component.secondFlightResponse = mockFlightSearchResponse;

    component.submitSelectedFlight(mockIndex);

    expect(mockUnwinderSessionService.setData).toHaveBeenCalledWith(
      'chosenSecondFlightData',
      mockFlightSearchResponse.data[mockIndex],
    );
  });
});
