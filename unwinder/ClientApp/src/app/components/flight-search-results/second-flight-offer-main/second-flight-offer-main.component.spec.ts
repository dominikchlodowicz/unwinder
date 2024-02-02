import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SecondFlightOfferMainComponent } from './second-flight-offer-main.component';

describe('SecondFlightOfferMainComponent', () => {
  let component: SecondFlightOfferMainComponent;
  let fixture: ComponentFixture<SecondFlightOfferMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SecondFlightOfferMainComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SecondFlightOfferMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
