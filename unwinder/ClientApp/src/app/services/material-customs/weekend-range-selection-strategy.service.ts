import { Injectable } from '@angular/core';
import {
  MatDateRangeSelectionStrategy,
  DateRange,
  MAT_DATE_RANGE_SELECTION_STRATEGY,
  MatDatepickerModule,
} from '@angular/material/datepicker';
import { DateAdapter, MatNativeDateModule } from '@angular/material/core';

@Injectable({
  providedIn: 'root',
})
export class WeekendRangeSelectionStategyService<D>
  implements MatDateRangeSelectionStrategy<D>
{
  constructor(private _dateAdapter: DateAdapter<D>) {}

  selectionFinished(date: D | null): DateRange<D> {
    return this._createWeekendDayRange(date);
  }

  createPreview(activeDate: D | null): DateRange<D> {
    return this._createWeekendDayRange(activeDate);
  }

  private _createWeekendDayRange(date: D | null): DateRange<D> {
    if (date) {
      const dayOfWeek = this._dateAdapter.getDayOfWeek(date);
      let start = null;
      let end = null;

      // Adjust if the selected day is Saturday or Sunday
      if (dayOfWeek === 0) {
        // Sunday
        start = this._dateAdapter.addCalendarDays(date, -1);
        end = this._dateAdapter.addCalendarDays(date, 0);
        console.log(`start: ${start}, end: ${end}`);
      } else if (dayOfWeek === 6) {
        // Saturday
        start = this._dateAdapter.addCalendarDays(date, 0);
        end = this._dateAdapter.addCalendarDays(date, 1);
        console.log(`start: ${start}, end: ${end}`);
      }

      return new DateRange<D>(start, end);
    }

    return new DateRange<D>(null, null);
  }
}
