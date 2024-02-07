import { Injectable } from '@angular/core';
import { IMondayCustomDateAdapterService } from '../../../interfaces/material-customs/date-adapter';

@Injectable({
  providedIn: 'root',
})
export class MondayCustomDateAdapterService
  implements IMondayCustomDateAdapterService
{
  getFirstDayOfWeek(): number {
    return 1;
  }
}
