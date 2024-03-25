import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { HotelSearchData } from '../../interfaces/hotel-data-exchange/hotel-search-data';
import { HotelResponseData } from '../../interfaces/hotel-data-exchange/hotel-response-data';

@Injectable({
  providedIn: 'root',
})
export class HotelSearchSubmitService {
  constructor(private httpClient: HttpClient) {}

  apiUrl = 'api/hotel-search';

  public serializeHotelSearchData(
    adultsArg: number,
    checkInArg: Date,
    checkOutArg: Date,
    cityCodeArg: string,
  ): HotelSearchData {
    return {
      adults: adultsArg,
      checkIn: checkInArg,
      checkOut: checkOutArg,
      cityCode: cityCodeArg,
    };
  }

  public submitHotelSearchDataToApi(
    data: HotelSearchData,
  ): Observable<HotelResponseData> {
    return this.httpClient.post<HotelResponseData>(this.apiUrl, data);
  }
}
