import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FlightOfferCardComponent } from './flight-offer-card.component';
import { ConcreteFlight } from '../../../interfaces/flight-data-exchange/concrete-flight';

describe('FlightOfferCardComponent', () => {
  let component: FlightOfferCardComponent;
  let fixture: ComponentFixture<FlightOfferCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FlightOfferCardComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(FlightOfferCardComponent);
    component = fixture.componentInstance;
  });

  it('should correctly initialize based on flightData input', () => {
    const mockFlightData: ConcreteFlight = {
      itineraries: [
        {
          duration: 'PT10H30M',
          segments: [
            {
              departure: {
                iataCode: 'LAX',
                at: '2024-02-05T07:00:00Z',
              },
              arrival: {
                iataCode: 'JFK',
                at: '2024-02-05T17:30:00Z',
              },
              duration: 'PT5H30M',
              carrierCode: 'DL',
              number: '123',
            },
          ],
        },
      ],
      price: {
        total: '450.00',
        currency: 'USD',
      },
    };

    component.flightData = mockFlightData;
    component.ngOnInit();

    expect(component.departureIataCode).toEqual('LAX');
    expect(component.arrivalIataCode).toEqual('JFK');
    expect(component.duration).toEqual('10h 30min');
    expect(component.totalPrice).toEqual('450.00 USD');
  });

  it('should emit indexOfRecievedData when selectFlight is called', () => {
    const selectSpy = jest.spyOn(component.selectedFlight, 'emit');
    const mockIndex = 1;

    component.indexOfRecievedData = mockIndex;
    component.selectFlight();

    expect(selectSpy).toHaveBeenCalledWith(mockIndex);
  });

  it('formats duration correctly with both hours and minutes', () => {
    const component = new FlightOfferCardComponent();
    const duration = 'PT10H30M';
    const formattedDuration = component.formatDuration(duration);
    expect(formattedDuration).toEqual('10h 30min');
  });

  it('formats duration correctly with only hours', () => {
    const component = new FlightOfferCardComponent();
    const duration = 'PT10H';
    const formattedDuration = component.formatDuration(duration);
    expect(formattedDuration).toEqual('10h ');
  });

  it('formats duration correctly with only minutes', () => {
    const component = new FlightOfferCardComponent();
    const duration = 'PT30M';
    const formattedDuration = component.formatDuration(duration);
    expect(formattedDuration).toEqual('30min');
  });

  it('should format departure and arrival times correctly', () => {
    const mockFlightData: ConcreteFlight = {
      itineraries: [
        {
          duration: 'PT10H30M',
          segments: [
            {
              departure: {
                iataCode: 'LAX',
                at: '2024-02-05T07:30:00Z',
              },
              arrival: {
                iataCode: 'JFK',
                at: '2024-02-05T15:45:00Z',
              },
              duration: 'PT5H30M',
              carrierCode: 'DL',
              number: '123',
            },
          ],
        },
      ],
      price: {
        total: '450.00',
        currency: 'USD',
      },
    };

    component.flightData = mockFlightData;
    component.ngOnInit();

    expect(component.departureTime).toEqual('08:30, 05.02');
    expect(component.arrivalTime).toEqual('16:45, 05.02');
  });
});
