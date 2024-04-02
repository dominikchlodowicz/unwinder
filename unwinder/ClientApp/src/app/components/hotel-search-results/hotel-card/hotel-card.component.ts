import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { HotelResponseOfferData } from '../../../interfaces/hotel-data-exchange/hotel-response-offer-data';

@Component({
  selector: 'app-hotel-card',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule, MatCardModule],
  templateUrl: './hotel-card.component.html',
  styleUrl: './hotel-card.component.scss',
})
export class HotelCardComponent {
  @Input() hotelData!: HotelResponseOfferData;
  googleMapsUrl: string = 'https://www.google.com/maps/place/';
  roomPhotoPath: string = '../../../../assets/hotel-room-photos/';

  photoPath?: string;
  hotelName?: string;
  googleMapsLink?: string;
  numberOfBeds?: number;
  checkIn?: string;
  checkOut?: string;
  price?: number;

  ngOnInit() {
    this.photoPath = this.drawPicture();
    this.hotelName = this.hotelData.hotel.name;
    this.googleMapsLink = `${this.googleMapsUrl}${this.hotelData.hotel.latitude}, ${this.hotelData.hotel.longitude}`;
    this.numberOfBeds = this.hotelData.offers[0].room.typeEstimated.beds;
    this.checkIn = this.hotelData.offers[0].checkInDate;
    this.checkOut = this.hotelData.offers[0].checkOutDate;
    this.price = this.hotelData.offers[0].convertedCurrencyPrice.value;
  }

  private drawPicture(): string {
    const randNumber: number = Math.floor(Math.random() * 5) + 1;

    return `${this.roomPhotoPath}${randNumber}.jpg`;
  }
}
