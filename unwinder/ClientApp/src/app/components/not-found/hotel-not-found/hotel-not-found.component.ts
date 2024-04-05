import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-hotel-not-found',
  standalone: true,
  imports: [CommonModule, MatCardModule],
  templateUrl: './hotel-not-found.component.html',
  styleUrl: './hotel-not-found.component.scss',
})
export class HotelNotFoundComponent {}
