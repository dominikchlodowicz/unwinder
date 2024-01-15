import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class FlightSearchCitiesService {
  constructor(private httpClient: HttpClient) {}

  apiUrl = '/api/flight-search';

  getCities(location: string): Observable<string[]> {
    return this.httpClient
      .get<string[]>(`${this.apiUrl}/get-city/${location}`)
      .pipe(
        catchError((error) => {
          console.error('Error fetching cities: ', error);
          // Return an empty array as an Observable in case of an error
          return of([]);
        }),
      );
  }
}
