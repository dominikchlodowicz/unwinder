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

  public submitFlightSearchDataToApi(
    data: FlightSearchData,
  ): Observable<FlightSearchResponse> {
    return this.httpClient.post<FlightSearchResponse>(this.apiUrl, data);
  }
}
