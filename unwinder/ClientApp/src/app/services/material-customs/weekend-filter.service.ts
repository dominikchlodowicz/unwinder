import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class WeekendFilterService {
  public isWeekend(d: Date | null): boolean {
    const day = (d || new Date()).getDay();
    // Allow only Saturday (6) and Sunday (0)
    return day === 0 || day === 6;
  }
}
