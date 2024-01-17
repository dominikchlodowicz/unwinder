import { MondayCustomDateAdapterService } from './monday-custom-date-adapter.service';

describe('MondayCustomDateAdapterService', () => {
  let service: MondayCustomDateAdapterService;

  beforeEach(() => {
    service = new MondayCustomDateAdapterService();
  });

  it('getFirstDayOfWeek should return 1', () => {
    expect(service.getFirstDayOfWeek()).toBe(1);
  });
});
