// injection-tokens.ts
import { InjectionToken } from '@angular/core';
import { IMondayCustomDateAdapterService } from '../interfaces/material-customs/date-adapter';
import { ICustomEuropeDateFormatService } from '../interfaces/material-customs/date-adapter';

export const MONDAY_CUSTOM_DATE_ADAPTER_SERVICE =
  new InjectionToken<IMondayCustomDateAdapterService>(
    'MondayCustomDateAdapterService',
  );
export const CUSTOM_EUROPE_DATE_FORMAT_SERVICE =
  new InjectionToken<ICustomEuropeDateFormatService>(
    'CustomEuropeDateFormatService',
  );
