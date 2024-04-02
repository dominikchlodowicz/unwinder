import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UnwinderSessionService } from '../../../services/unwinder-search-state/unwinder-session.service';
import { FlightOfferCardComponent } from '../flight-offer-card/flight-offer-card.component';
import { FlightSearchResponse } from '../../../interfaces/flight-data-exchange/flight-search-response';
import { HotelSearchSubmitService } from '../../../services/hotel-search/hotel-search-submit.service';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { HotelSearchData } from '../../../interfaces/hotel-data-exchange/hotel-search-data';
import { Router } from '@angular/router';
import { forkJoin, switchMap } from 'rxjs';

@Component({
  selector: 'app-second-flight-offer-main',
  standalone: true,
  imports: [CommonModule, FlightOfferCardComponent, MatProgressSpinnerModule],
  templateUrl: './second-flight-offer-main.component.html',
  styleUrl: './second-flight-offer-main.component.scss',
})
export class SecondFlightOfferMainComponent {
  secondFlightResponse!: FlightSearchResponse;
  isLoading = false;

  constructor(
    public _unwinderSessionService: UnwinderSessionService,
    public _hotelSearchSubmitService: HotelSearchSubmitService,
    private router: Router,
  ) {}

  ngOnInit() {
    this._unwinderSessionService
      .getSessionDataObservable()
      .subscribe((data) => {
        this.secondFlightResponse = data!.secondFlightResponse!;
      });
  }

  submitSelectedFlight(indexOfSelectedFlight: number) {
    this.isLoading = true;
    const selctedData = this.secondFlightResponse.data[indexOfSelectedFlight];

    const setChosenFlight$ = this._unwinderSessionService.setData(
      'chosenSecondFlightData',
      selctedData,
    );

    const hotelSearchData: HotelSearchData =
      this._hotelSearchSubmitService.serializeHotelSearchData(
        this._unwinderSessionService.getData('flightParameters')
          .numberOfPassengers,
        this._unwinderSessionService.getData('flightParameters').when,
        this._unwinderSessionService.getData('flightBackDate'),
        this._unwinderSessionService.getData('flightParameters').where,
      );

    const setHotelResponse$ = this._hotelSearchSubmitService
      .submitHotelSearchDataToApi(hotelSearchData)
      .pipe(
        switchMap((response) =>
          this._unwinderSessionService.setData('hotelResponse', response),
        ),
      );

    forkJoin([setChosenFlight$, setHotelResponse$]).subscribe({
      next: () => {
        this.router.navigate(['/unwind/hotel']);
        this.isLoading = false;
      },
      error: (error) => console.error('Error updating data:', error),
    });
  }
}
