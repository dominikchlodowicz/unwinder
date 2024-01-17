import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { WhereToFlightSearchFormComponent } from './where-to-flight-search-form.component';
import { FlightSearchCitiesService } from '../../../services/flight-search-form/flight-search-cities.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { of } from 'rxjs';
import { fakeAsync, tick, flush } from '@angular/core/testing';

describe('WhereToFlightSearchComponent', () => {
  let component: WhereToFlightSearchFormComponent;
  let fixture: ComponentFixture<WhereToFlightSearchFormComponent>;
  let mockFlightSearchCitiesService: jest.Mocked<FlightSearchCitiesService>;

  beforeEach(async () => {
    mockFlightSearchCitiesService = {
      getCities: jest.fn(),
    } as unknown as jest.Mocked<FlightSearchCitiesService>;

    await TestBed.configureTestingModule({
      imports: [
        WhereToFlightSearchFormComponent,
        ReactiveFormsModule,
        BrowserAnimationsModule,
      ],
      providers: [
        {
          provide: FlightSearchCitiesService,
          useValue: mockFlightSearchCitiesService,
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(WhereToFlightSearchFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create and initialize form control', () => {
    expect(component).toBeTruthy();
    expect(component.citiesAutocompleteWhereTo).toBeInstanceOf(FormControl);
    expect(component.citiesAutocompleteWhereTo.valid).toBeFalsy();
  });

  it('should filter cities based on service response', fakeAsync(() => {
    const mockCities = ['New York', 'Los Angeles', 'Chicago'];
    mockFlightSearchCitiesService.getCities.mockReturnValue(of(mockCities));

    component.citiesAutocompleteWhereTo.setValue('New');
    tick(300);
    fixture.detectChanges();

    expect(component.filteredCities).toEqual(['New York']);
    flush();
  }));

  it('should return the citiesAutocompleteWhereTo form control', () => {
    expect(component.whereToElementFormControl).toBe(
      component.citiesAutocompleteWhereTo,
    );
  });

  it('should filter response cities based on search string', () => {
    component.responseCities = ['New York', 'Los Angeles', 'Chicago', 'Newark'];

    let filtered = component.filterValues('New');
    expect(filtered).toEqual(['New York', 'Newark']);

    filtered = component.filterValues('Los');
    expect(filtered).toEqual(['Los Angeles']);
  });

  it('should set selectedFromDropdown to true when onOptionSelected is called', () => {
    expect(component['selectedFromDropdown']).toBe(false);
    component.onOptionSelected();
    expect(component['selectedFromDropdown']).toBe(true);
  });
});
