import { EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';

export abstract class BaseWhichWeekendFlightSearchFormComponent {
  abstract whichWeekendRange: FormGroup;
  abstract defaultToggleValue: string;
  abstract weekendFilter: (d: Date | null) => boolean;

  abstract startHour: number;
  abstract endHour: number;

  weekendTypeChange: EventEmitter<string> = new EventEmitter<string>();

  // exposed getter for main-flight component
  get whichWeekendRangeFormControl(): FormGroup {
    return this.whichWeekendRange;
  }

  // inform main component about type change
  changeWeekendType(weekendType: string): void {
    this.weekendTypeChange.emit(weekendType);
  }

  setHoursForDateRange(): void {
    const startDateValue = this.whichWeekendRange.get('start')!.value;
    const endDateValue = this.whichWeekendRange.get('end')!.value;

    if (startDateValue) {
      const startDate = new Date(startDateValue);
      startDate.setHours(this.startHour, 0, 0, 0);
      this.whichWeekendRange.get('start')!.setValue(startDate);
    }

    if (endDateValue) {
      const endDate = new Date(endDateValue);
      endDate.setHours(this.endHour, 0, 0, 0);
      this.whichWeekendRange.get('end')!.setValue(endDate);
    }
  }

  subscribeToValueChanges() {
    this.whichWeekendRange.get('start')!.valueChanges.subscribe((date) => {
      if (date) {
        this.updateDate('start', date);
      }
    });

    this.whichWeekendRange.get('end')!.valueChanges.subscribe((date) => {
      if (date) {
        this.updateDate('end', date);
      }
    });
  }

  updateDate(controlName: 'start' | 'end', newDate: Date) {
    const adjustedDate = this.adjustDateToLocalTimezone(newDate);
    // Polish timezone correction
    const hours =
      controlName === 'start' ? this.startHour + 1 : this.endHour + 1;
    adjustedDate.setHours(hours, 0, 0, 0);

    // Get current value to compare
    const currentValue = this.whichWeekendRange.get(controlName)!.value;
    if (!this.isSameDate(currentValue, adjustedDate)) {
      this.whichWeekendRange
        .get(controlName)!
        .patchValue(adjustedDate, { emitEvent: false });
    }
  }

  adjustDateToLocalTimezone(date: Date): Date {
    const localTime = new Date(date);
    localTime.setMinutes(date.getMinutes() - date.getTimezoneOffset());
    return localTime;
  }

  public isSameDate(date1: Date, date2: Date): boolean {
    return (
      date1.getHours() === date2.getHours() &&
      date1.getMinutes() === date2.getMinutes() &&
      date1.getSeconds() === date2.getSeconds() &&
      date1.getMilliseconds() === date2.getMilliseconds()
    );
  }
}
