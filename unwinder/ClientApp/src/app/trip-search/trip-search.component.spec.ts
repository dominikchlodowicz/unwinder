import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TripSearchComponent } from './trip-search.component';

describe('FlightSearchComponent', () => {
  let component: TripSearchComponent;
  let fixture: ComponentFixture<TripSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ TripSearchComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TripSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
