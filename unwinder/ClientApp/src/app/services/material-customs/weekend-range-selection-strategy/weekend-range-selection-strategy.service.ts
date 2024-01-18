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

      if (dayOfWeek === 0) {
        // Sunday
        start = this._dateAdapter.addCalendarDays(date, -2); // Friday
        end = date; // Sunday
      } else if (dayOfWeek === 6) {
        // Saturday
        start = this._dateAdapter.addCalendarDays(date, -1); // Friday
        end = this._dateAdapter.addCalendarDays(date, 1); // Sunday
      } else if (dayOfWeek === 5) {
        // Friday
        start = date; // Friday
        end = this._dateAdapter.addCalendarDays(date, 2); // Sunday
      }

      // If the selected date is Friday, Saturday, or Sunday
      if (start !== null && end !== null) {
        return new DateRange<D>(start, end);
      }
    }

    return new DateRange<D>(null, null);
  }
}
