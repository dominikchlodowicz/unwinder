import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CustomDateFormatService {
  myFormats = {
    parse: {
      dateInput: 'LL',
    },
    display: {
      dateInput: 'YYYY-MM-DD',
      monthYearLabel: 'YYYY',
      dateA11yLabel: 'LL',
      monthYearA11yLabel: 'YYYY',
    },
  };

  getFormats() {
    // You can add logic here to modify the formats if needed
    return this.myFormats;
  }
}
