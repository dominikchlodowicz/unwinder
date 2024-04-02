import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
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
      checkIn: checkInArg.toISOString().split('T')[0],
      checkOut: checkOutArg.toISOString().split('T')[0],
      cityCode: cityCodeArg,
    };
  }

  public submitHotelSearchDataToApi(
    data: HotelSearchData,
  ): Observable<HotelResponseData> {
    let params = new HttpParams()
      .set('adults', data.adults.toString())
      .set('checkIn', data.checkIn.toString())
      .set('checkOut', data.checkOut.toString())
      .set('cityCode', data.cityCode);

    return this.httpClient.get<HotelResponseData>(this.apiUrl, {
      params: params,
    });
  }
}
