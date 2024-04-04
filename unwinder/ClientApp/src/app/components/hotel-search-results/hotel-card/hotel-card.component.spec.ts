import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { HotelCardComponent } from './hotel-card.component';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

describe('HotelCardComponent', () => {
  let component: HotelCardComponent;
  let fixture: ComponentFixture<HotelCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HotelCardComponent,
        MatIconModule,
        MatButtonModule,
        MatCardModule,
        NoopAnimationsModule,
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(HotelCardComponent);
    component = fixture.componentInstance;
    component.hotelData = {
      type: 'hotel',
      hotel: {
        hotelId: 'testHotel123',
        name: 'Test Hotel',
        latitude: 40.712776,
        longitude: -74.005974,
      },
      available: true,
      offers: [
        {
          id: 'offer123',
          checkInDate: '2024-04-04',
          checkOutDate: '2024-04-06',
          room: {
            typeEstimated: {
              category: 'Deluxe',
              beds: 2,
            },
          },
          price: {
            currency: 'USD',
            total: '200.00',
          },
          convertedCurrencyPrice: {
            currencyCode: 'USD',
            value: 200,
          },
        },
      ],
      self: 'http://example.com/self/link',
    };

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
