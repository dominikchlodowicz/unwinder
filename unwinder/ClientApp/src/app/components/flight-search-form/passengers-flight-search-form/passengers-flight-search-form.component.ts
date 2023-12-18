import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSliderModule } from '@angular/material/slider';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-passengers-flight-search-form',
  standalone: true,
  imports: [CommonModule, MatSliderModule],
  templateUrl: './passengers-flight-search-form.component.html',
  styleUrl: './passengers-flight-search-form.component.css'
})
export class PassengersFlightSearchFormComponent {
  private passengerSlider: FormControl = new FormControl();

  formatLabel(value: number): string {
    return `${value}`;
  }

  get passengerSliderFormControl(): FormControl {
    return this.passengerSlider;
  }
}
