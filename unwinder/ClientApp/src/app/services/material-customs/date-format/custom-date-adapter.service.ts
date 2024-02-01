import { NativeDateAdapter, MAT_DATE_LOCALE } from '@angular/material/core';
import { Injectable, Inject } from '@angular/core';

import {
  IMondayCustomDateAdapterService,
  ICustomEuropeDateFormatService,
} from '../../../interfaces/material-customs/date-adapter';
import {
  CUSTOM_EUROPE_DATE_FORMAT_SERVICE,
  MONDAY_CUSTOM_DATE_ADAPTER_SERVICE,
} from '../../../injection-tokens/material-injection-tokens';

@Injectable({ providedIn: 'root' })
export class CustomDateAdapterService extends NativeDateAdapter {
  constructor(
    @Inject(MONDAY_CUSTOM_DATE_ADAPTER_SERVICE)
    private mondayAdapter: IMondayCustomDateAdapterService,
    @Inject(CUSTOM_EUROPE_DATE_FORMAT_SERVICE)
    private europeAdapter: ICustomEuropeDateFormatService,
    @Inject(MAT_DATE_LOCALE) matDateLocale: string,
  ) {
    super();
    this.setLocale(matDateLocale);
  }

  override getFirstDayOfWeek(): number {
    return this.mondayAdapter.getFirstDayOfWeek();
  }

  override parse(value: any, parseFormat: any): Date | null {
    return this.europeAdapter.parse(value);
  }
}
