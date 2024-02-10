import { TestBed } from '@angular/core/testing';
import { WeekendFilterService } from './weekend-filter.service';

describe('WeekendFilterService', () => {
  let service: WeekendFilterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WeekendFilterService);
  });

  // Tests for isShortWeekend
  describe('isShortWeekend', () => {
    it('should return true for future weekends (Saturday and Sunday)', () => {
      const futureSaturday = new Date();
      futureSaturday.setDate(
        futureSaturday.getDate() + ((6 - futureSaturday.getDay()) % 7),
      ); // Next Saturday
      futureSaturday.setHours(0, 0, 0, 0);

      const futureSunday = new Date(futureSaturday);
      futureSunday.setDate(futureSunday.getDate() + 1); // Next Sunday

      expect(service.isShortWeekend(futureSaturday)).toBe(true);
      expect(service.isShortWeekend(futureSunday)).toBe(true);
    });

    it('should return false for weekdays', () => {
      const monday = new Date('2023-01-09');
      expect(service.isShortWeekend(monday)).toBe(false);
    });

    it('should return false for past weekends', () => {
      const pastSaturday = new Date('2023-01-07'); // Known past Saturday
      const pastSunday = new Date('2023-01-08'); // Known past Sunday
      expect(service.isShortWeekend(pastSaturday)).toBe(false);
      expect(service.isShortWeekend(pastSunday)).toBe(false);
    });

    it('should handle null dates', () => {
      const result = service.isShortWeekend(null);
      const today = new Date();
      today.setHours(0, 0, 0, 0);
      const dayOfWeek = today.getDay();
      const expected = (dayOfWeek === 0 || dayOfWeek === 6) && today >= today;
      expect(result).toBe(expected);
    });
  });

  // Tests for isLongWeekend
  describe('isLongWeekend', () => {
    it('should return true for future long weekends (Friday, Saturday, and Sunday)', () => {
      const futureFriday = new Date();
      futureFriday.setDate(
        futureFriday.getDate() + ((5 - futureFriday.getDay() + 7) % 7 || 7),
      ); // Next Friday
      futureFriday.setHours(0, 0, 0, 0);

      const futureSaturday = new Date(futureFriday);
      futureSaturday.setDate(futureSaturday.getDate() + 1); // Next Saturday

      const futureSunday = new Date(futureSaturday);
      futureSunday.setDate(futureSunday.getDate() + 1); // Next Sunday

      expect(service.isLongWeekend(futureFriday)).toBe(true);
      expect(service.isLongWeekend(futureSaturday)).toBe(true);
      expect(service.isLongWeekend(futureSunday)).toBe(true);
    });

    it('should return false for weekdays', () => {
      const tuesday = new Date('2023-01-10');
      expect(service.isLongWeekend(tuesday)).toBe(false);
    });

    it('should return false for past long weekends', () => {
      const pastFriday = new Date('2023-01-06'); // Known past Friday
      const pastSaturday = new Date('2023-01-07'); // Known past Saturday
      const pastSunday = new Date('2023-01-08'); // Known past Sunday
      expect(service.isLongWeekend(pastFriday)).toBe(false);
      expect(service.isLongWeekend(pastSaturday)).toBe(false);
      expect(service.isLongWeekend(pastSunday)).toBe(false);
    });

    it('should handle null dates', () => {
      const result = service.isLongWeekend(null);
      const today = new Date();
      today.setHours(0, 0, 0, 0);
      const dayOfWeek = today.getDay();
      const expected =
        (dayOfWeek === 5 || dayOfWeek === 6 || dayOfWeek === 0) &&
        today >= today;
      expect(result).toBe(expected);
    });
  });
});
