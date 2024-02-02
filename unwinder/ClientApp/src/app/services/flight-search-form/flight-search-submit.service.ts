import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { FlightDate } from '../../interfaces/flight-data-exchange/flight-date';
import { FlightSearchData } from '../../interfaces/flight-data-exchange/flight-search-data';
import { FlightSearchResponse } from '../../interfaces/flight-data-exchange/flight-search-response';

@Injectable({
  providedIn: 'root',
})
export class FlightSearchSubmitService {
  constructor(private httpClient: HttpClient) {}

  apiUrl = '/api/flight-search';

  public serializeFlightSearchData(
    whereArg: string,
    originArg: string,
    whenArg: Date,
    passengersArg: number,
  ): FlightSearchData {
    return {
      where: whereArg,
      origin: originArg,
      when: whenArg,
      numberOfPassengers: passengersArg,
    };
  }

  public splitIsoDateString(dateString: string) {
    const date = new Date(dateString);
    // Check if the date is valid
    if (isNaN(date.getTime())) {
      throw new Error('Invalid date string');
    }

    // YYYY-MM-DD
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0'); // Months are zero-based
    const day = date.getDate().toString().padStart(2, '0');
    const splittedDate = `${year}-${month}-${day}`;

    // hh:mm:ss
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');
    const seconds = date.getSeconds().toString().padStart(2, '0');
    const splittedTime = `${hours}:${minutes}:${seconds}`;
    return { splittedDate, splittedTime };
  }

  public serializeFlightBackData(flightBackDate: string): FlightDate {
    let splittedDates = this.splitIsoDateString(flightBackDate);
    return {
      flightTime: splittedDates.splittedDate,
      flightDate: splittedDates.splittedTime,
    };
  }

  public submitFlightSearchDataToApi(
    data: FlightSearchData,
  ): Observable<FlightSearchResponse> {
    return this.httpClient.post<FlightSearchResponse>(this.apiUrl, data);
  }
}
