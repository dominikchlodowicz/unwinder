import { TestBed } from '@angular/core/testing';
import { ShortWeekendRangeSelectionStategyService } from './short-weekend-range-selection-strategy.service';
import { DateAdapter, MatNativeDateModule } from '@angular/material/core';

describe('WeekendRangeSelectionStategyService', () => {
  let service: ShortWeekendRangeSelectionStategyService<Date>;
  let dateAdapter: DateAdapter<Date>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [MatNativeDateModule],
      providers: [ShortWeekendRangeSelectionStategyService],
    });

    service = TestBed.inject(ShortWeekendRangeSelectionStategyService);
    dateAdapter = TestBed.inject(DateAdapter);
  });

  it('should create correct range for Saturday', () => {
    const saturday = dateAdapter.createDate(2023, 0, 7); // 7th Jan 2023 is a Saturday
    const range = service.selectionFinished(saturday);

    // Expecting Saturday and the next day (Sunday)
    expect(dateAdapter.sameDate(range.start, saturday)).toBe(true);
    expect(
      dateAdapter.sameDate(range.end, dateAdapter.addCalendarDays(saturday, 1)),
    ).toBe(true);
  });

  it('should create correct range for Sunday', () => {
    const sunday = dateAdapter.createDate(2023, 0, 8); // 8th Jan 2023 is a Sunday
    const range = service.selectionFinished(sunday);

    // Expecting Sunday and the previous day (Saturday)
    expect(
      dateAdapter.sameDate(
        range.start,
        dateAdapter.addCalendarDays(sunday, -1),
      ),
    ).toBe(true);
    expect(dateAdapter.sameDate(range.end, sunday)).toBe(true);
  });

  it('should return null range for a weekday', () => {
    const weekday = dateAdapter.createDate(2023, 0, 5); // 5th Jan 2023 is a weekday
    const range = service.selectionFinished(weekday);

    expect(range.start).toBeNull();
    expect(range.end).toBeNull();
  });

  it('should return null range for a null date', () => {
    const range = service.selectionFinished(null);
    expect(range.start).toBeNull();
    expect(range.end).toBeNull();
  });

  it('should create correct preview range for Saturday', () => {
    const saturday = dateAdapter.createDate(2023, 0, 7); // 7th Jan 2023 is a Saturday
    const range = service.createPreview(saturday);

    expect(dateAdapter.sameDate(range.start, saturday)).toBe(true);
    expect(
      dateAdapter.sameDate(range.end, dateAdapter.addCalendarDays(saturday, 1)),
    ).toBe(true);
  });

  it('should create correct preview range for Sunday', () => {
    const sunday = dateAdapter.createDate(2023, 0, 8); // 8th Jan 2023 is a Sunday
    const range = service.createPreview(sunday);

    expect(
      dateAdapter.sameDate(
        range.start,
        dateAdapter.addCalendarDays(sunday, -1),
      ),
    ).toBe(true);
    expect(dateAdapter.sameDate(range.end, sunday)).toBe(true);
  });

  it('should return null preview range for a weekday', () => {
    const weekday = dateAdapter.createDate(2023, 0, 5); // 5th Jan 2023 is a weekday
    const range = service.createPreview(weekday);

    expect(range.start).toBeNull();
    expect(range.end).toBeNull();
  });

  it('should return null preview range for a null date', () => {
    const range = service.createPreview(null);
    expect(range.start).toBeNull();
    expect(range.end).toBeNull();
  });
});
