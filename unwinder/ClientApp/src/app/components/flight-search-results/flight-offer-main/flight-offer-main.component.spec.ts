import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FlightOfferMainComponent } from './flight-offer-main.component';

describe('FlightOfferMainComponent', () => {
  let component: FlightOfferMainComponent;
  let fixture: ComponentFixture<FlightOfferMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FlightOfferMainComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(FlightOfferMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
