import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FlightNotFoundComponent } from './flight-not-found.component';

describe('FlightNotFoundComponent', () => {
  let component: FlightNotFoundComponent;
  let fixture: ComponentFixture<FlightNotFoundComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FlightNotFoundComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(FlightNotFoundComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
