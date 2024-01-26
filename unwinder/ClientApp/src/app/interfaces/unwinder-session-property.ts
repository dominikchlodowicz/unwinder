import { FlightDate } from './flight-date';

export interface UnwinderSessionProperty {
  flightBackData: FlightDate;
  firstFlightResponse: string;
  chosenFirstFlightData: string;
  secondFlightResponse: string;
  chosenSecondFlightData: string;
  hotelResponse: string;
  chosenHotelData: string;
}

export type UnwinderSessionPropertyKey = keyof UnwinderSessionProperty;
