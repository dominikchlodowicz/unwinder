import { TestBed } from '@angular/core/testing';
import { LongWeekendRangeSelectionStategyService } from './long-weekend-range-selection-strategy.service';
import { DateAdapter, MatNativeDateModule } from '@angular/material/core';

describe('LongWeekendRangeSelectionStategyService', () => {
  let service: LongWeekendRangeSelectionStategyService<Date>;
  let dateAdapter: DateAdapter<Date>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [MatNativeDateModule],
      providers: [LongWeekendRangeSelectionStategyService],
    });
    service = TestBed.inject(LongWeekendRangeSelectionStategyService);
    dateAdapter = TestBed.inject(DateAdapter);
  });

  // Helper function to create a Date object

  it('should create correct range for Friday', () => {
    const friday = dateAdapter.createDate(2023, 0, 6);
    const range = service.selectionFinished(friday);

    // Expecting Friday, Saturday and Sunday
    expect(dateAdapter.sameDate(range.start, friday)).toBe(true);
    expect(
      dateAdapter.sameDate(range.end, dateAdapter.addCalendarDays(friday, 2)),
    ).toBe(true);
  });

  it('should create correct range for Saturday', () => {
    const saturday = dateAdapter.createDate(2023, 0, 7);
    const range = service.selectionFinished(saturday);

    expect(
      dateAdapter.sameDate(
        range.start,
        dateAdapter.addCalendarDays(saturday, -1),
      ),
    ).toBe(true);
    expect(
      dateAdapter.sameDate(range.end, dateAdapter.addCalendarDays(saturday, 1)),
    ).toBe(true);
  });

  it('should create correct range for Sunday', () => {
    const sunday = dateAdapter.createDate(2023, 0, 8);
    const range = service.selectionFinished(sunday);

    expect(
      dateAdapter.sameDate(
        range.start,
        dateAdapter.addCalendarDays(sunday, -2),
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
