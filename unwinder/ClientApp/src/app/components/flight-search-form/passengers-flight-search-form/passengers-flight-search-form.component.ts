import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSliderModule } from '@angular/material/slider';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-passengers-flight-search-form',
  standalone: true,
  imports: [CommonModule, MatSliderModule, ReactiveFormsModule],
  templateUrl: './passengers-flight-search-form.component.html',
  styleUrl: './passengers-flight-search-form.component.css',
})
export class PassengersFlightSearchFormComponent {
  passengerSlider: FormControl = new FormControl(1);

  formatLabel(value: number): string {
    return `${value}`;
  }

  public get passengerSliderFormControl(): FormControl {
    return this.passengerSlider;
  }
}
