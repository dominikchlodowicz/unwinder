import { Injectable } from '@angular/core';
import { ICustomEuropeDateFormatService } from '../../../interfaces/date-adapter';

@Injectable({
  providedIn: 'root',
})
export class CustomEuropeDateFormatService
  implements ICustomEuropeDateFormatService
{
  // This method Parses timestamp into 'DD-MM-YYYY' format
  parse(timestampInput: any): Date | null {
    if (
      typeof timestampInput === 'string' &&
      timestampInput.indexOf('/') > -1
    ) {
      const str = timestampInput.split('/');
      // Validate that the string can be split into three parts and all parts are numbers
      if (
        str.length < 2 ||
        isNaN(+str[0]) ||
        isNaN(+str[1]) ||
        isNaN(+str[2])
      ) {
        return null;
      }
      // Create a new Date object using the parts of the string
      // Note: Months are 0-indexed in JavaScript Dates (0 = January, 11 = December)
      return new Date(Number(str[2]), Number(str[1]) - 1, Number(str[0]), 24);
    }
    const timestamp =
      typeof timestampInput === 'number'
        ? timestampInput
        : Date.parse(timestampInput);
    return isNaN(timestamp) ? null : new Date(timestamp);
  }
}
