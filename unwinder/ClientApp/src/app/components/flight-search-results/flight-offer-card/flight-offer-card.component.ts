import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { ConcreteFlight } from '../../../interfaces/flight-data-exchange/concrete-flight';

@Component({
  selector: 'app-flight-offer-card',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule],
  templateUrl: './flight-offer-card.component.html',
  styleUrl: './flight-offer-card.component.scss',
})
export class FlightOfferCardComponent {
  @Input() flightData!: ConcreteFlight;
  @Input() indexOfRecievedData!: number;
  @Output() selectedFlight = new EventEmitter<number>();

  departureIataCode?: string;
  arrivalIataCode?: string;
  layovers?: number;
  departureTime?: string;
  arrivalTime?: string;
  duration?: string;
  operatorNames?: string;
  totalPrice?: string;

  ngOnInit() {
    const itineraries = this.flightData.itineraries;
    const segments = itineraries[0].segments;
    const lastItinerary = itineraries[itineraries.length - 1];
    const lastSegment =
      lastItinerary.segments[lastItinerary.segments.length - 1];

    this.departureIataCode = segments[0].departure.iataCode;
    this.arrivalIataCode = lastSegment.arrival.iataCode;
    this.layovers = segments.length - 1;
    this.departureTime = this.formatTime(
      this.flightData.itineraries[0].segments[0].departure.at,
    );
    this.arrivalTime = this.formatTime(lastSegment.arrival.at);
    this.duration = this.formatDuration(itineraries[0].duration);

    const operatorSet = new Set(segments.map((seg) => seg.carrierCode));
    this.operatorNames = Array.from(operatorSet).join(', ');

    this.totalPrice = `${this.flightData.price.total} ${this.flightData.price.currency}`;
  }

  private formatTime(isoString: string): string {
    const date = new Date(isoString);
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    return `${hours}:${minutes}, ${day}.${month}`;
  }

  public formatDuration(durationString: string): string {
    const hoursMatch = durationString.match(/(\d+)H/);
    const minutesMatch = durationString.match(/(\d+)M/);
    const hours = hoursMatch ? `${hoursMatch[1]}h ` : '';
    const minutes = minutesMatch ? `${minutesMatch[1]}min` : '';
    return hours + minutes;
  }

  public selectFlight() {
    this.selectedFlight.emit(this.indexOfRecievedData);
  }
}
