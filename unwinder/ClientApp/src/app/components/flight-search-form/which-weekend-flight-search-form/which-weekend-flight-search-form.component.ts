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
import { WeekendRangeSelectionStategyService } from '../../../services/material-customs/weekend-range-selection-strategy.service';
import { MondayCustomDateAdapterService } from '../../../services/material-customs/monday-custom-date-adapter.service';
import { CustomDateFormatService } from '../../../services/material-customs/custom-date-format.service';

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
      useClass: WeekendRangeSelectionStategyService,
    },
    {
      provide: DateAdapter,
      useClass: MondayCustomDateAdapterService,
    },
    { 
      provide: MAT_DATE_FORMATS,
      useFactory: (customDateFormatService: CustomDateFormatService) => customDateFormatService.getFormats(),
      deps: [CustomDateFormatService],
    },
  ],
  templateUrl: './which-weekend-flight-search-form.component.html',
  styleUrl: './which-weekend-flight-search-form.component.css',
})
export class WhichWeekendFlightSearchComponent {
  weekendFilter = (d: Date | null): boolean => {
    const day = (d || new Date()).getDay();
    console.log('test');
    // Allow only Saturday (6) and Sunday (0)
    return day === 0 || day === 6;
  };
}
