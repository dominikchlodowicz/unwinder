import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FirstFlightOfferMainComponent } from './first-flight-offer-main.component';

describe('FlightOfferMainComponent', () => {
  let component: FirstFlightOfferMainComponent;
  let fixture: ComponentFixture<FirstFlightOfferMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FirstFlightOfferMainComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(FirstFlightOfferMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
