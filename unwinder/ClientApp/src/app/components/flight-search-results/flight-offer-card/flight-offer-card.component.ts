import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UnwinderSessionService } from '../../../services/unwinder-search-state/unwinder-session.service';

@Component({
  selector: 'app-flight-offer-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './flight-offer-card.component.html',
  styleUrl: './flight-offer-card.component.css',
})
export class FlightOfferCardComponent {
  constructor(private _unwinderSessionService: UnwinderSessionService) {}

  flightBackData = this._unwinderSessionService.getData('flightBackData');
}
