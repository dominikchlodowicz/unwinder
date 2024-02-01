import { FlightSearchData } from './flight-search-data';
import { FlightSearchResponse } from './flight-search-response';

export interface UnwinderSessionProperty {
  flightParameters?: FlightSearchData;
  flightBackData?: Date;
  firstFlightResponse?: FlightSearchResponse;
  chosenFirstFlightData?: string;
  secondFlightResponse?: FlightSearchResponse;
  chosenSecondFlightData?: string;
  hotelResponse?: string;
  chosenHotelData?: string;
}

export type UnwinderSessionPropertyKey = keyof UnwinderSessionProperty;
