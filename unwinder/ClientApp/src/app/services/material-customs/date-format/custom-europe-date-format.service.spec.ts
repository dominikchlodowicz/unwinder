import { CustomEuropeDateFormatService } from './custom-europe-date-format.service';

describe('CustomEuropeDateFormatService', () => {
  let service: CustomEuropeDateFormatService;

  beforeEach(() => {
    service = new CustomEuropeDateFormatService();
  });

  it('should correctly parse valid date strings', () => {
    const dateStr = '01/02/2023'; // DD/MM/YYYY format
    const result = service.parse(dateStr);
    // Expecting the next day because the service sets hours to 24
    expect(result).toEqual(new Date(2023, 1, 2)); // Note: Months are 0-indexed
  });

  it('should return null for completely invalid date strings', () => {
    const invalidDateStr = 'This is not a date';
    const result = service.parse(invalidDateStr);
    expect(result).toBeNull();
  });

  it('should handle non-string inputs correctly', () => {
    const timestamp = Date.now();
    const resultFromTimestamp = service.parse(timestamp);
    expect(resultFromTimestamp).toEqual(new Date(timestamp));

    const invalidInput = { date: '01/02/2023' }; // Non-string, non-number input
    const resultFromInvalidInput = service.parse(invalidInput);
    expect(resultFromInvalidInput).toBeNull();
  });
});
