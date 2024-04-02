import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HotelResponseData } from '../../../interfaces/hotel-data-exchange/hotel-response-data';
import { UnwinderSessionService } from '../../../services/unwinder-search-state/unwinder-session.service';
import { HotelCardComponent } from '../hotel-card/hotel-card.component';

@Component({
  selector: 'app-hotel-search-results',
  standalone: true,
  imports: [CommonModule, HotelCardComponent],
  templateUrl: './hotel-search-results.component.html',
  styleUrl: './hotel-search-results.component.scss',
})
export class HotelSearchResultsComponent {
  hotelSearchOutput!: HotelResponseData;

  constructor(public _unwinderSessionService: UnwinderSessionService) {}

  ngOnInit() {
    this.hotelSearchOutput =
      this._unwinderSessionService.getData('hotelResponse');
  }
}
