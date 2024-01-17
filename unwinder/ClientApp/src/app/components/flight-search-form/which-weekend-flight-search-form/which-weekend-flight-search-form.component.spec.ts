import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { WhichWeekendFlightSearchComponent } from './which-weekend-flight-search-form.component';
import { WeekendFilterService } from '../../../services/material-customs/weekend-filter.service';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import {
  MatDateRangeInputHarness,
  MatEndDateHarness,
  MatStartDateHarness,
} from '@angular/material/datepicker/testing';
import { HarnessLoader } from '@angular/cdk/testing';
import { TestbedHarnessEnvironment } from '@angular/cdk/testing/testbed';

describe('WhichWeekendFlightSearchFormComponent', () => {
  let component: WhichWeekendFlightSearchComponent;
  let fixture: ComponentFixture<WhichWeekendFlightSearchComponent>;
  let mockWeekendFilterService: jest.Mocked<WeekendFilterService>;
  let loader: HarnessLoader;

  beforeEach(waitForAsync(() => {
    mockWeekendFilterService = {
      isWeekend: jest.fn(),
    };

    TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        MatDatepickerModule,
        MatInputModule,
        MatNativeDateModule,
        NoopAnimationsModule,
        WhichWeekendFlightSearchComponent,
      ],
      providers: [
        { provide: WeekendFilterService, useValue: mockWeekendFilterService },
        // Mock other services if necessary
      ],
    })
      .compileComponents()
      .then(async () => {
        fixture = TestBed.createComponent(WhichWeekendFlightSearchComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
        loader = TestbedHarnessEnvironment.loader(fixture);
      });
  }));

  it('should create and initialize form controls', () => {
    expect(component).toBeTruthy();
    expect(component.whichWeekendRangeFormControl).toBeInstanceOf(FormGroup);
    expect(component.whichWeekendRangeFormControl.get('start')).toBeInstanceOf(
      FormControl,
    );
    expect(component.whichWeekendRangeFormControl.get('end')).toBeInstanceOf(
      FormControl,
    );
  });

  it('should update form control values when dates are selected', async () => {
    const inputHarness = await loader.getHarness(MatDateRangeInputHarness);
    const startDateInput = await loader.getHarness(MatStartDateHarness);
    const endDateInput = await loader.getHarness(MatEndDateHarness);

    await startDateInput.setValue('01/01/2022');

    expect(
      component.whichWeekendRangeFormControl
        .get('start')!
        .value.toISOString()
        .substring(0, 10),
    ).toEqual('2022-01-01');
  });

  it('should open date picker and select a date range', async () => {
    fixture.detectChanges();
    await fixture.whenStable();

    const dateRangeInput = await loader.getHarness(MatDateRangeInputHarness);
    const startDateInput = await loader.getHarness(MatStartDateHarness);
    const endDateInput = await loader.getHarness(MatEndDateHarness);

    await startDateInput.setValue('01/01/2022'); // Set start date
    await endDateInput.setValue('01/07/2022'); // Set end date

    expect(
      component.whichWeekendRangeFormControl
        .get('start')!
        .value.toISOString()
        .substring(0, 10),
    ).toEqual('2022-01-01');
    expect(
      component.whichWeekendRangeFormControl
        .get('end')!
        .value.toISOString()
        .substring(0, 10),
    ).toEqual('2022-07-01');
  });
});
