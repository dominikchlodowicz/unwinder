import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  MAT_DATE_RANGE_SELECTION_STRATEGY,
  MatDateRangePicker,
  MatDatepickerModule,
} from '@angular/material/datepicker';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatInputModule } from '@angular/material/input';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { DateAdapter, MatNativeDateModule } from '@angular/material/core';
import { MAT_DATE_LOCALE } from '@angular/material/core';

import { ShortWeekendRangeSelectionStategyService } from '../../../services/material-customs/weekend-range-selection-strategy/short/short-weekend-range-selection-strategy.service';
import { MondayCustomDateAdapterService } from '../../../services/material-customs/date-format/monday-custom-date-adapter.service';
import { CustomEuropeDateFormatService } from '../../../services/material-customs/date-format/custom-europe-date-format.service';
import { CustomDateAdapterService } from '../../../services/material-customs/date-format/custom-date-adapter.service';
import {
  CUSTOM_EUROPE_DATE_FORMAT_SERVICE,
  MONDAY_CUSTOM_DATE_ADAPTER_SERVICE,
} from '../../../injection-tokens/material-injection-tokens';
import { WeekendFilterService } from '../../../services/material-customs/weekend-filter.service';
import { BaseWhichWeekendFlightSearchFormComponent } from '../base-which-weekend-flight-search-form/base-which-weekend-flight-search-form.component';

@Component({
  selector: 'app-which-weekend',
  standalone: true,
  imports: [
    CommonModule,
    MatDatepickerModule,
    MatInputModule,
    ReactiveFormsModule,
    MatNativeDateModule,
    MatButtonToggleModule,
  ],
  providers: [
    {
      provide: MAT_DATE_RANGE_SELECTION_STRATEGY,
      useClass: ShortWeekendRangeSelectionStategyService,
    },
    {
      provide: MAT_DATE_LOCALE,
      useValue: 'en-GB',
    },
    {
      provide: MONDAY_CUSTOM_DATE_ADAPTER_SERVICE,
      useClass: MondayCustomDateAdapterService,
    },
    {
      provide: CUSTOM_EUROPE_DATE_FORMAT_SERVICE,
      useClass: CustomEuropeDateFormatService,
    },
    {
      provide: DateAdapter,
      useClass: CustomDateAdapterService,
      deps: [
        MONDAY_CUSTOM_DATE_ADAPTER_SERVICE,
        CUSTOM_EUROPE_DATE_FORMAT_SERVICE,
        MAT_DATE_LOCALE,
      ],
    },
  ],
  templateUrl: './short-which-weekend-flight-search-form.component.html',
  styleUrl: './short-which-weekend-flight-search-form.component.css',
})
export class ShortWhichWeekendFlightSearchComponent extends BaseWhichWeekendFlightSearchFormComponent {
  @ViewChild(MatDateRangePicker) picker!: MatDateRangePicker<Date>;
  @Output() override weekendTypeChange = new EventEmitter<string>();
  constructor(private weekendFilterService: WeekendFilterService) {
    super();
  }

  startHour: number = 1;
  endHour: number = 18;

  whichWeekendRange = new FormGroup({
    start: new FormControl<Date | null>(null, [Validators.required]),
    end: new FormControl<Date | null>(null, [Validators.required]),
  });

  ngOnInit() {
    this.setHoursForDateRange();
    this.subscribeToValueChanges();
  }

  defaultToggleValue = 'short';

  weekendFilter = this.weekendFilterService.isShortWeekend;
}
