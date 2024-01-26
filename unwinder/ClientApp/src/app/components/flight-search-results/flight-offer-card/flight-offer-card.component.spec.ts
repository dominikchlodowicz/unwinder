import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FlightOfferCardComponent } from './flight-offer-card.component';

describe('FlightOfferCardComponent', () => {
  let component: FlightOfferCardComponent;
  let fixture: ComponentFixture<FlightOfferCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FlightOfferCardComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(FlightOfferCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
