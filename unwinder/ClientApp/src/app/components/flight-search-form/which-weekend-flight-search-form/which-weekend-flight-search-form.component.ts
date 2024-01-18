import { Component, InjectionToken, ViewChild } from '@angular/core';
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

import { WeekendRangeSelectionStategyService } from '../../../services/material-customs/weekend-range-selection-strategy/weekend-range-selection-strategy.service';
import { LongWeekendRangeSelectionStategyService } from '../../../services/material-customs/weekend-range-selection-strategy/long-weekend-range-selection-strategy.service';
import { MondayCustomDateAdapterService } from '../../../services/material-customs/date-format/monday-custom-date-adapter.service';
import { CustomEuropeDateFormatService } from '../../../services/material-customs/date-format/custom-europe-date-format.service';
import { CustomDateAdapterService } from '../../../services/material-customs/date-format/custom-date-adapter.service';
import {
  CUSTOM_EUROPE_DATE_FORMAT_SERVICE,
  MONDAY_CUSTOM_DATE_ADAPTER_SERVICE,
  PROVIDERS_MAP,
} from '../../../injection-tokens/material-injection-tokens';
import { WeekendFilterService } from '../../../services/material-customs/weekend-filter.service';
import {
  DynamicProviderSwitchService,
  dynamicProviderFactory,
} from '../../../services/provider-switch/provider-switch.service';

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
    // {
    //   provide: MAT_DATE_RANGE_SELECTION_STRATEGY,
    //   useClass: WeekendRangeSelectionStategyService,
    // },
    {
      provide: PROVIDERS_MAP,
      useValue: {
        short: WeekendRangeSelectionStategyService,
        long: LongWeekendRangeSelectionStategyService,
      },
    },
    {
      provide: MAT_DATE_RANGE_SELECTION_STRATEGY,
      useFactory: dynamicProviderFactory,
      deps: [DynamicProviderSwitchService, PROVIDERS_MAP],
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
  templateUrl: './which-weekend-flight-search-form.component.html',
  styleUrl: './which-weekend-flight-search-form.component.css',
})
export class WhichWeekendFlightSearchComponent {
  @ViewChild(MatDateRangePicker) picker!: MatDateRangePicker<Date>;
  constructor(
    private weekendFilterService: WeekendFilterService,
    private providerSwitchService: DynamicProviderSwitchService,
  ) {}

  switchProvider(toggleValue: string) {
    switch (toggleValue) {
      case 'long':
        console.log('long');
        this.providerSwitchService.setProviderKey('long');
        break;
      case 'short':
        console.log('short');
        this.providerSwitchService.setProviderKey('short');
        break;
      default:
        break;
    }
  }

  defaultToggleValue = 'short';

  weekendFilter = this.weekendFilterService.isWeekend;

  whichWeekendRange = new FormGroup({
    start: new FormControl<Date | null>(null, [Validators.required]),
    end: new FormControl<Date | null>(null, [Validators.required]),
  });

  get whichWeekendRangeFormControl(): FormGroup {
    return this.whichWeekendRange;
  }
}
