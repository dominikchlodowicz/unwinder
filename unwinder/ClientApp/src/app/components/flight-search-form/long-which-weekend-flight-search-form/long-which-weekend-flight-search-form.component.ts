import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import {
  MatNativeDateModule,
  MAT_DATE_LOCALE,
  DateAdapter,
} from '@angular/material/core';
import {
  MatDatepickerModule,
  MAT_DATE_RANGE_SELECTION_STRATEGY,
  MatDateRangePicker,
} from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import {
  MONDAY_CUSTOM_DATE_ADAPTER_SERVICE,
  CUSTOM_EUROPE_DATE_FORMAT_SERVICE,
} from '../../../injection-tokens/material-injection-tokens';
import { CustomDateAdapterService } from '../../../services/material-customs/date-format/custom-date-adapter.service';
import { CustomEuropeDateFormatService } from '../../../services/material-customs/date-format/custom-europe-date-format.service';
import { MondayCustomDateAdapterService } from '../../../services/material-customs/date-format/monday-custom-date-adapter.service';
import { WeekendFilterService } from '../../../services/material-customs/weekend-filter.service';
import { BaseWhichWeekendFlightSearchFormComponent } from '../base-which-weekend-flight-search-form/base-which-weekend-flight-search-form.component';
import { LongWeekendRangeSelectionStategyService } from '../../../services/material-customs/weekend-range-selection-strategy/long/long-weekend-range-selection-strategy.service';

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
      useClass: LongWeekendRangeSelectionStategyService,
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
  templateUrl: './long-which-weekend-flight-search-form.component.html',
  styleUrl: './long-which-weekend-flight-search-form.component.scss',
})
export class LongWhichWeekendFlightSearchFormComponent extends BaseWhichWeekendFlightSearchFormComponent {
  @ViewChild(MatDateRangePicker) picker!: MatDateRangePicker<Date>;
  @Output() override weekendTypeChange = new EventEmitter<string>();
  constructor(private weekendFilterService: WeekendFilterService) {
    super();
  }

  startHour: number = 15;
  endHour: number = 18;

  whichWeekendRange = new FormGroup({
    start: new FormControl<Date | null>(null, [Validators.required]),
    end: new FormControl<Date | null>(null, [Validators.required]),
  });

  ngOnInit() {
    this.setHoursForDateRange();
    this.subscribeToValueChanges();
  }

  defaultToggleValue = 'long';

  weekendFilter = this.weekendFilterService.isLongWeekend;
}
