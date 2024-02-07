import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UnwinderSessionService } from '../../../services/unwinder-search-state/unwinder-session.service';
import { FlightOfferCardComponent } from '../flight-offer-card/flight-offer-card.component';
import { FlightSearchResponse } from '../../../interfaces/flight-data-exchange/flight-search-response';

@Component({
  selector: 'app-second-flight-offer-main',
  standalone: true,
  imports: [CommonModule, FlightOfferCardComponent],
  templateUrl: './second-flight-offer-main.component.html',
  styleUrl: './second-flight-offer-main.component.css',
})
export class SecondFlightOfferMainComponent {
  secondFlightResponse!: FlightSearchResponse;

  constructor(public _unwinderSessionService: UnwinderSessionService) {}

  ngOnInit() {
    this._unwinderSessionService
      .getSessionDataObservable()
      .subscribe((data) => {
        this.secondFlightResponse = data!.secondFlightResponse!;
      });
  }

  submitSelectedFlight(indexOfSelectedFlight: number) {
    const selctedData = this.secondFlightResponse.data[indexOfSelectedFlight];

    this._unwinderSessionService.setData('chosenSecondFlightData', selctedData);
  }
}
