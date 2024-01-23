import { DateRange } from '@angular/material/datepicker';

export abstract class BaseWeekendRangeSelectionStategyService<D> {
  abstract selectionFinished(date: D | null): DateRange<D>;

  abstract createPreview(activeDate: D | null): DateRange<D>;

  protected abstract _createWeekendDayRange(date: D | null): DateRange<D>;
}
