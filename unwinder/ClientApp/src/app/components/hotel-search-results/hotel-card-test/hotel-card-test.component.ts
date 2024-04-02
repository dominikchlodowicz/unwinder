import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { HotelResponseOfferData } from '../../../interfaces/hotel-data-exchange/hotel-response-offer-data';

@Component({
  selector: 'app-hotel-card-test',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule, MatCardModule],
  templateUrl: './hotel-card-test.component.html',
  styleUrl: './hotel-card-test.component.scss',
})
export class HotelCardTestComponent {}
