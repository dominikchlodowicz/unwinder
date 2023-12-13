import { Component, Injectable } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  DateRange,
  MAT_DATE_RANGE_SELECTION_STRATEGY,
  MatDatepickerModule,
} from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { ReactiveFormsModule } from '@angular/forms';
import { DateAdapter, MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { FlightSearchWeekendRangeSelectionStrategyService } from '../../../services/flight-search-form/flight-search-weekend-range-selection-strategy.service';

@Component({
  selector: 'app-which-weekend',
  standalone: true,
  imports: [
    CommonModule,
    MatDatepickerModule,
    MatInputModule,
    ReactiveFormsModule,
    MatNativeDateModule,
  ],
  providers: [
    {
      provide: MAT_DATE_RANGE_SELECTION_STRATEGY,
      useClass: FlightSearchWeekendRangeSelectionStrategyService,
    },
  ],
  templateUrl: './which-weekend.component.html',
  styleUrl: './which-weekend.component.css',
})
export class WhichWeekendFlightSearchComponent {
  weekendFilter = (d: Date | null): boolean => {
    const day = (d || new Date()).getDay();
    console.log('test');
    // Allow only Saturday (6) and Sunday (0)
    return day === 0 || day === 6;
  };

  //TODO: Date range selector color change, Make monday as a first day of the week.
}
