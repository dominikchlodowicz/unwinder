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

//  // it('should submit when all forms are valid', () => {
//   component.ngOnInit();
//   spyOn(
//     component._flightSearchSubmitService,
//     'submitFlightSearchDataToApi',
//   ).and.returnValue(of({}));
//   component.whereFormGroup.setControl('where', new FormControl('test'));
//   component.originFormGroup.setControl('origin', new FormControl('test'));
//   component.whenFormGroup.setControl(
//     'when',
//     new FormControl({ start: new Date(), end: new Date() }),
//   );
//   component.passengersFormGroup.setControl('slider', new FormControl(1));

//   component.submit();
//   expect(
//     component._flightSearchSubmitService.submitFlightSearchDataToApi,
//   ).toHaveBeenCalled();
// });
