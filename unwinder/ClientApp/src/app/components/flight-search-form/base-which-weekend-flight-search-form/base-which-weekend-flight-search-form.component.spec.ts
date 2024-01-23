import { TestBed, ComponentFixture } from '@angular/core/testing';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Component } from '@angular/core';
import { BaseWhichWeekendFlightSearchFormComponent } from './base-which-weekend-flight-search-form.component';

@Component({ template: '' })
class DummyWeekendFlightSearchComponent extends BaseWhichWeekendFlightSearchFormComponent {
  whichWeekendRange = new FormGroup({
    start: new FormControl(),
    end: new FormControl(),
  });
  defaultToggleValue = 'dummy';
  weekendFilter = (d: Date | null) => true;
  startHour = 9;
  endHour = 17;

  get testStartHour() {
    return this.startHour;
  }

  get testEndHour() {
    return this.endHour;
  }
}

describe('BaseWhichWeekendFlightSearchFormComponent', () => {
  let component: DummyWeekendFlightSearchComponent;
  let fixture: ComponentFixture<DummyWeekendFlightSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DummyWeekendFlightSearchComponent],
      imports: [ReactiveFormsModule],
    }).compileComponents();

    fixture = TestBed.createComponent(DummyWeekendFlightSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('adjustDateToLocalTimezone', () => {
    it('should correctly adjust a UTC date to the local timezone', () => {
      const testDate = new Date('2020-01-01T12:00:00Z'); // Noon UTC
      const adjustedDate = component.adjustDateToLocalTimezone(testDate);
      expect(adjustedDate).toBeInstanceOf(Date);
      expect(adjustedDate.getHours()).not.toBe(12);
    });
  });

  it('should emit weekendTypeChange when changeWeekendType is called', () => {
    jest.spyOn(component.weekendTypeChange, 'emit');
    component.changeWeekendType('long');
    expect(component.weekendTypeChange.emit).toHaveBeenCalledWith('long');
  });

  describe('setHoursForDateRange', () => {
    it('should correctly set the hours for start and end dates', () => {
      component.whichWeekendRange
        .get('start')!
        .setValue(new Date('2020-01-01T00:00:00Z'));
      component.whichWeekendRange
        .get('end')!
        .setValue(new Date('2020-01-01T00:00:00Z'));
      component.setHoursForDateRange();
      expect(component.whichWeekendRange.get('start')!.value.getHours()).toBe(
        component.testStartHour,
      );
      expect(component.whichWeekendRange.get('end')!.value.getHours()).toBe(
        component.testEndHour,
      );
    });

    it('should handle null start and/or end date values', () => {
      component.whichWeekendRange.get('start')!.setValue(null);
      component.whichWeekendRange.get('end')!.setValue(null);
      component.setHoursForDateRange();
      expect(component.whichWeekendRange.get('start')!.value).toBeNull();
      expect(component.whichWeekendRange.get('end')!.value).toBeNull();
    });
  });

  // whichWeekendRangeFormControl Getter
  it('should return the whichWeekendRange form group', () => {
    expect(component.whichWeekendRangeFormControl).toBeInstanceOf(FormGroup);
    expect(component.whichWeekendRangeFormControl).toBe(
      component.whichWeekendRange,
    );
  });

  describe('Form Controls Initialization and Value Changes', () => {
    it('should correctly initialize form controls', () => {
      expect(component.whichWeekendRange.get('start')!.value).toBeNull();
      expect(component.whichWeekendRange.get('end')!.value).toBeNull();
    });

    it('should update start and end date values on valueChanges', () => {
      const startDate = new Date('2020-01-01T00:00:00Z');
      const endDate = new Date('2020-01-02T00:00:00Z');
      component.whichWeekendRange.get('start')!.setValue(startDate);
      component.whichWeekendRange.get('end')!.setValue(endDate);
      expect(component.whichWeekendRange.get('start')!.value).toEqual(
        startDate,
      );
      expect(component.whichWeekendRange.get('end')!.value).toEqual(endDate);
    });

    it('should handle invalid or null dates provided', () => {
      component.whichWeekendRange.get('start')!.setValue(undefined);
      component.whichWeekendRange.get('end')!.setValue('invalid-date');
      expect(component.whichWeekendRange.get('start')!.value).toBeUndefined();
      expect(component.whichWeekendRange.get('end')!.value).toBe(
        'invalid-date',
      );
    });
  });

  it('should correctly update the form control values', () => {
    component.whichWeekendRange
      .get('start')!
      .setValue(new Date('2020-01-01T00:00:00Z'));
    const newDate = new Date('2020-01-01T12:00:00Z');
    component.updateDate('start', newDate);
    const expectedDate = new Date('2020-01-01T09:00:00Z');
    expect(component.whichWeekendRange.get('start')!.value).toEqual(
      expectedDate,
    );
  });

  describe('isSameDate', () => {
    it('should identify when two dates are the same', () => {
      const date1 = new Date('2020-01-01T12:00:00');
      const date2 = new Date('2020-01-01T12:00:00');
      expect(component.isSameDate(date1, date2)).toBeTruthy();
    });

    it('should differentiate dates that differ by hour, minute, second, millisecond', () => {
      const date1 = new Date('2020-01-01T12:00:00');
      const date2 = new Date('2020-01-01T13:01:01.001');
      expect(component.isSameDate(date1, date2)).toBeFalsy();
    });
  });

  it('should have implemented defaultToggleValue and weekendFilter as expected', () => {
    expect(component.defaultToggleValue).toBe('dummy');
    expect(component.weekendFilter(new Date())).toBeTruthy();
  });

  it('should work correctly for weekendTypeChange EventEmitter', () => {
    jest.spyOn(component.weekendTypeChange, 'emit');
    component.changeWeekendType('new-type');
    expect(component.weekendTypeChange.emit).toHaveBeenCalledWith('new-type');
  });

  describe('subscribeToValueChanges', () => {
    it('should update start date on value change', () => {
      const newStartDate = new Date('2020-01-01T09:00:00Z');
      component.whichWeekendRange.get('start')!.setValue(newStartDate);
      expect(component.whichWeekendRange.get('start')!.value).toEqual(
        newStartDate,
      );
      // Additionally, check if the time has been adjusted to startHour
    });

    it('should update end date on value change', () => {
      const newEndDate = new Date('2020-01-01T17:00:00Z');
      component.whichWeekendRange.get('end')!.setValue(newEndDate);
      expect(component.whichWeekendRange.get('end')!.value).toEqual(newEndDate);
      // Additionally, check if the time has been adjusted to endHour
    });
  });
});
