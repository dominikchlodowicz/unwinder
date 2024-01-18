import { WeekendFilterService } from './weekend-filter.service';

describe('WeekendFilterService', () => {
  let service: WeekendFilterService;

  beforeEach(() => {
    service = new WeekendFilterService();
  });

  it('should return true for weekends that are today or in the future', () => {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    const dayOfWeek = today.getDay();

    // Check if today is a weekend and in the future
    if (dayOfWeek === 0 || dayOfWeek === 6) {
      expect(service.isWeekend(today)).toBe(true);
    } else {
      // Find next Saturday
      const nextSaturday = new Date();
      nextSaturday.setDate(today.getDate() + (6 - dayOfWeek));
      expect(service.isWeekend(nextSaturday)).toBe(true);
    }
  });

  it('should return false for weekdays', () => {
    // Create a known Monday
    const monday = new Date('2023-01-09');
    expect(service.isWeekend(monday)).toBe(false);
  });

  it('should return false for past weekends', () => {
    // Create past weekend dates
    const pastSaturday = new Date('2023-01-07'); // Known past Saturday
    const pastSunday = new Date('2023-01-08'); // Known past Sunday

    expect(service.isWeekend(pastSaturday)).toBe(false);
    expect(service.isWeekend(pastSunday)).toBe(false);
  });

  it('should return true or false based on the current day if date is null', () => {
    const result = service.isWeekend(null);
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    const dayOfWeek = today.getDay();
    const expected = (dayOfWeek === 0 || dayOfWeek === 6) && today >= today;

    expect(result).toBe(expected);
  });
});
