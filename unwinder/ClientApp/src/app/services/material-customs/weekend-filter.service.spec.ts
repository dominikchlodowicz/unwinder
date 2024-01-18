import { WeekendFilterService } from './weekend-filter.service';

describe('WeekendFilterService', () => {
  let service: WeekendFilterService;

  beforeEach(() => {
    service = new WeekendFilterService();
  });

  it('should return true for weekends', () => {
    // Create dates for Saturday and Sunday
    const saturday = new Date('2023-01-07'); // Adjust the date to a known Saturday
    const sunday = new Date('2023-01-08'); // Adjust the date to a known Sunday

    expect(service.isWeekend(saturday)).toBe(true);
    expect(service.isWeekend(sunday)).toBe(true);
  });

  it('should return false for weekdays', () => {
    const monday = new Date('2023-01-09'); // Known Monday
    expect(service.isWeekend(monday)).toBe(false);
  });

  it('should return true or false based on the current day', () => {
    const result = service.isWeekend(null);
    const today = new Date().getDay();
    const expected = today === 0 || today === 6;
    expect(result).toBe(expected);
  });
});
