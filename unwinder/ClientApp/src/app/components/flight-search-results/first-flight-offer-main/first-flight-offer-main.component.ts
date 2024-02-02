import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UnwinderSessionService } from '../../../services/unwinder-search-state/unwinder-session.service';
import { FlightOfferCardComponent } from '../flight-offer-card/flight-offer-card.component';
import { FlightSearchSubmitService } from '../../../services/flight-search-form/flight-search-submit.service';
import { FlightSearchData } from '../../../interfaces/flight-data-exchange/flight-search-data';
import { FlightSearchResponse } from '../../../interfaces/flight-data-exchange/flight-search-response';
import { switchMap } from 'rxjs/internal/operators/switchMap';
import { forkJoin } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-flight-offer-main',
  standalone: true,
  imports: [CommonModule, FlightOfferCardComponent],
  templateUrl: './first-flight-offer-main.component.html',
  styleUrl: './first-flight-offer-main.component.css',
})
export class FirstFlightOfferMainComponent {
  flightBackData!: Date;
  firstFlightResponse!: FlightSearchResponse;
  flightParameters!: FlightSearchData;

  constructor(
    public _unwinderSessionService: UnwinderSessionService,
    private _flightSearchSubmitService: FlightSearchSubmitService,
    private router: Router,
  ) {}

  ngOnInit() {
    this._unwinderSessionService
      .getSessionDataObservable()
      .subscribe((data) => {
        this.flightBackData = data!.flightBackData!;
        this.firstFlightResponse = data!.firstFlightResponse!;
        this.flightParameters = data!.flightParameters!;
      });
  }

  submitSelectedFlight(indexOfSelectedFlight: number) {
    const selctedData = this.firstFlightResponse.data[indexOfSelectedFlight];

    const serializedFlightSearchData: FlightSearchData =
      this._flightSearchSubmitService.serializeFlightSearchData(
        this.flightParameters.origin,
        this.flightParameters.where,
        this.flightBackData,
        this.flightParameters.numberOfPassengers,
      );

    const setChosenFlight$ = this._unwinderSessionService.setData(
      'chosenFirstFlightData',
      selctedData,
    );

    const setChosenSecondFlightData$ = this._flightSearchSubmitService
      .submitFlightSearchDataToApi(serializedFlightSearchData)
      .pipe(
        switchMap((response) =>
          this._unwinderSessionService.setData(
            'secondFlightResponse',
            response,
          ),
        ),
      );

    forkJoin([setChosenFlight$, setChosenSecondFlightData$]).subscribe({
      next: () => {
        this.router.navigate(['/unwind/second-flight']);
      },
      error: (error) => console.error('Error updating data:', error),
    });
  }
}
