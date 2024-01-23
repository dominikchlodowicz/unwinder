import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PassengersFlightSearchFormComponent } from './passengers-flight-search-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('PassengersFlightSearchFormComponent', () => {
  let component: PassengersFlightSearchFormComponent;
  let fixture: ComponentFixture<PassengersFlightSearchFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        PassengersFlightSearchFormComponent,
        ReactiveFormsModule,
        BrowserAnimationsModule,
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(PassengersFlightSearchFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should return the passengerSliderFormControl', () => {
    expect(component.passengerSliderFormControl).toBe(
      component.passengerSlider,
    );
  });
});
