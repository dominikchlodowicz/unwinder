import { Injectable } from '@angular/core';
import { IMondayCustomDateAdapterService } from '../../../interfaces/date-adapter';

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
