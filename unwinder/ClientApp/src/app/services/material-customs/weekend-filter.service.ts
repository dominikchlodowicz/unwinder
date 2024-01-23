import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class WeekendFilterService {
  public isShortWeekend(d: Date | null): boolean {
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    const dateToCheck = d ? new Date(d) : today;
    dateToCheck.setHours(0, 0, 0, 0);

    const dayOfWeek = dateToCheck.getDay();
    const isWeekend = dayOfWeek === 0 || dayOfWeek === 6;

    //future check
    const isTodayOrFuture = dateToCheck >= today;

    return isWeekend && isTodayOrFuture;
  }

  public isLongWeekend(d: Date | null): boolean {
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    const dateToCheck = d ? new Date(d) : today;
    dateToCheck.setHours(0, 0, 0, 0);

    const dayOfWeek = dateToCheck.getDay();
    const isWeekend = dayOfWeek === 5 || dayOfWeek === 6 || dayOfWeek === 0; // Including Friday

    //future check
    const isTodayOrFuture = dateToCheck >= today;

    return isWeekend && isTodayOrFuture;
  }
}
