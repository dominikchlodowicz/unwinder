import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { LongWhichWeekendFlightSearchFormComponent } from './long-which-weekend-flight-search-form.component';
import { WeekendFilterService } from '../../../services/material-customs/weekend-filter.service';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { HarnessLoader } from '@angular/cdk/testing';
import { TestbedHarnessEnvironment } from '@angular/cdk/testing/testbed';

describe('LongWhichWeekendFlightSearchFormComponent', () => {
  let component: LongWhichWeekendFlightSearchFormComponent;
  let fixture: ComponentFixture<LongWhichWeekendFlightSearchFormComponent>;
  let mockWeekendFilterService: jest.Mocked<WeekendFilterService>;
  let loader: HarnessLoader;

  beforeEach(waitForAsync(() => {
    mockWeekendFilterService = {
      isShortWeekend: jest.fn(),
      isLongWeekend: jest.fn(),
    };

    TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        MatDatepickerModule,
        MatInputModule,
        MatNativeDateModule,
        NoopAnimationsModule,
        LongWhichWeekendFlightSearchFormComponent,
      ],
      providers: [
        { provide: WeekendFilterService, useValue: mockWeekendFilterService },
      ],
    })
      .compileComponents()
      .then(() => {
        fixture = TestBed.createComponent(
          LongWhichWeekendFlightSearchFormComponent,
        );
        component = fixture.componentInstance;
        fixture.detectChanges();
        loader = TestbedHarnessEnvironment.loader(fixture);
      });
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form controls', () => {
    expect(component.whichWeekendRange instanceof FormGroup).toBe(true);
    expect(
      component.whichWeekendRange.get('start') instanceof FormControl,
    ).toBe(true);
    expect(component.whichWeekendRange.get('end') instanceof FormControl).toBe(
      true,
    );
  });

  it('should set correct default values for form controls', () => {
    expect(component.whichWeekendRange.get('start')!.value).toBeNull();
    expect(component.whichWeekendRange.get('end')!.value).toBeNull();
  });

  it('should emit weekendTypeChange when changeWeekendType is called', () => {
    jest.spyOn(component.weekendTypeChange, 'emit');
    component.changeWeekendType('long');
    expect(component.weekendTypeChange.emit).toHaveBeenCalledWith('long');
  });

  it('should set hours for date range correctly', () => {
    const testStartDate = new Date();
    const testEndDate = new Date();

    component.whichWeekendRange.get('start')!.setValue(testStartDate);
    component.whichWeekendRange.get('end')!.setValue(testEndDate);

    component.setHoursForDateRange();

    expect(component.whichWeekendRange.get('start')!.value!.getHours()).toBe(
      component.startHour + 1,
    );
    expect(component.whichWeekendRange.get('end')!.value!.getHours()).toBe(
      component.endHour + 1,
    );
  });

  it('should update form values on valueChanges', () => {
    const newStartDate = new Date();
    const newEndDate = new Date();

    component.whichWeekendRange
      .get('start')!
      .patchValue(newStartDate, { emitEvent: false });
    component.whichWeekendRange
      .get('end')!
      .patchValue(newEndDate, { emitEvent: false });

    component.whichWeekendRange.get('start')!.setValue(newStartDate);
    component.whichWeekendRange.get('end')!.setValue(newEndDate);

    const expectedStartHours = component.startHour + 1;
    const expectedEndHours = component.endHour + 1;

    expect(component.whichWeekendRange.get('start')!.value!.getHours()).toBe(
      expectedStartHours,
    );
    expect(component.whichWeekendRange.get('end')!.value!.getHours()).toBe(
      expectedEndHours,
    );
  });
});
