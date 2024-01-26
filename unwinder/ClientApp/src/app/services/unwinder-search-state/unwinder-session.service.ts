import { Injectable } from '@angular/core';
import {
  UnwinderSessionProperty,
  UnwinderSessionPropertyKey,
} from '../../interfaces/unwinder-session-property';

@Injectable({
  providedIn: 'root',
})
export class UnwinderSessionService {
  private sessionData: UnwinderSessionProperty = {
    flightBackData: {},
    firstFlightResponse: '',
    chosenFirstFlightData: '',
    secondFlightResponse: '',
    chosenSecondFlightData: '',
    hotelResponse: '',
    chosenHotelData: '',
  };

  constructor() {}

  setData(property: UnwinderSessionPropertyKey, data: any) {
    this.sessionData[property] = data;
  }

  getData(property: UnwinderSessionPropertyKey): any {
    return this.sessionData[property];
  }

  clearData(property: UnwinderSessionPropertyKey) {
    if (property in this.sessionData) {
      // Reset to default state based on property type
      if (property === 'flightBackData') {
        this.sessionData[property] = {};
      } else {
        this.sessionData[property] = '';
      }
    }
  }
}
