import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-flight-not-found',
  standalone: true,
  imports: [CommonModule, MatCardModule],
  templateUrl: './flight-not-found.component.html',
  styleUrl: './flight-not-found.component.scss',
})
export class FlightNotFoundComponent {}
