import { Injectable } from '@angular/core';
// import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { NativeDateAdapter } from '@angular/material/core';
import { MAT_DATE_LOCALE } from '@angular/material/core';

@Injectable({
  providedIn: 'root'
})
export class MondayCustomDateAdapterService extends NativeDateAdapter{
  override getFirstDayOfWeek(): number {
    return 1;
  }
}
