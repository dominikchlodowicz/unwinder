import { FlightSearchData } from './flight-search-data';
import { FlightSearchResponse } from './flight-search-response';
import { HotelResponseData } from '../hotel-data-exchange/hotel-response-data';

export interface UnwinderSessionProperty {
  flightParameters?: FlightSearchData;
  flightBackDate?: Date;
  firstFlightResponse?: FlightSearchResponse;
  chosenFirstFlightData?: string;
  secondFlightResponse?: FlightSearchResponse;
  chosenSecondFlightData?: string;
  hotelResponse?: HotelResponseData;
  chosenHotelData?: string;
}

export type UnwinderSessionPropertyKey = keyof UnwinderSessionProperty;
