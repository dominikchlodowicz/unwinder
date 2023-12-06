import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class FlightSearchCitiesService {
  constructor(private httpClient: HttpClient) {}

  apiUrl = '/api/flight-search';

  getCities(cityName: string): Observable<string[]> {
    return this.httpClient.get<string[]>(
      `${this.apiUrl}/get-airport/${location}`,
    );
  }
}
