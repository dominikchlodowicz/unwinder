import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FlightSearchData } from '../../interfaces/flight-search-data';
import { Observable, catchError } from 'rxjs';

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
    backArg: Date,
    passengersArg: number,
  ): FlightSearchData {
    return {
      where: whereArg,
      origin: originArg,
      when: whenArg,
      back: backArg,
      numberOfPassengers: passengersArg,
    };
  }

  public submitFlightSearchDataToApi(
    data: FlightSearchData,
  ): Observable<FlightSearchData> {
    console.log('submit');
    return this.httpClient.post<FlightSearchData>(this.apiUrl, data);
  }
}
