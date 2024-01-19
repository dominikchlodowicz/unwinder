import {
  ChangeDetectorRef,
  Component,
  ComponentFactoryResolver,
  Injector,
  OnInit,
  Type,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';

import { WhereToFlightSearchFormComponent } from '../where-to-flight-search-form/where-to-flight-search-form.component';
import { WhichWeekendFlightSearchComponent } from '../which-weekend-flight-search-form/which-weekend-flight-search-form.component';
import { OriginFlightSearchFormComponent } from '../origin-flight-search-form/origin-flight-search-form.component';
import { PassengersFlightSearchFormComponent } from '../passengers-flight-search-form/passengers-flight-search-form.component';

import { MatStepperModule } from '@angular/material/stepper';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSnackBar } from '@angular/material/snack-bar';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { MatIconModule } from '@angular/material/icon';
import { FlightSearchData } from '../../../interfaces/flight-search-data';
import { FlightSearchSubmitService } from '../../../services/flight-search-form/flight-search-submit.service';
import { MAT_DATE_RANGE_SELECTION_STRATEGY } from '@angular/material/datepicker';
import { LongWeekendRangeSelectionStategyService } from '../../../services/material-customs/weekend-range-selection-strategy/long-weekend-range-selection-strategy.service';
import { WeekendRangeSelectionStategyService } from '../../../services/material-customs/weekend-range-selection-strategy/weekend-range-selection-strategy.service';
import { DateAdapter, MAT_DATE_LOCALE } from '@angular/material/core';
import {
  MONDAY_CUSTOM_DATE_ADAPTER_SERVICE,
  CUSTOM_EUROPE_DATE_FORMAT_SERVICE,
} from '../../../injection-tokens/material-injection-tokens';
import { CustomDateAdapterService } from '../../../services/material-customs/date-format/custom-date-adapter.service';
import { CustomEuropeDateFormatService } from '../../../services/material-customs/date-format/custom-europe-date-format.service';
import { MondayCustomDateAdapterService } from '../../../services/material-customs/date-format/monday-custom-date-adapter.service';

@Component({
  selector: 'app-main-flight-search-form',
  standalone: true,
  imports: [
    CommonModule,
    WhereToFlightSearchFormComponent,
    WhichWeekendFlightSearchComponent,
    OriginFlightSearchFormComponent,
    PassengersFlightSearchFormComponent,
    MatStepperModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
  ],
  templateUrl: './main-flight-search-form.component.html',
  styleUrl: './main-flight-search-form.component.css',
  providers: [
    {
      provide: STEPPER_GLOBAL_OPTIONS,
      useValue: { showError: true },
    },
  ],
})
export class MainFlightSearchFormComponent implements OnInit {
  @ViewChild(WhereToFlightSearchFormComponent)
  public citiesAutocompleteWhereTo!: WhereToFlightSearchFormComponent;

  @ViewChild(OriginFlightSearchFormComponent)
  public originFlightSearchFormComponent!: OriginFlightSearchFormComponent;

  @ViewChild(PassengersFlightSearchFormComponent)
  public passengersFlightSearchFormComponent!: PassengersFlightSearchFormComponent;

  @ViewChild('weekendComponentContainer', { read: ViewContainerRef })
  private weekendComponentContainer!: ViewContainerRef;

  whereFormGroup!: FormGroup;
  originFormGroup!: FormGroup;
  whenFormGroup!: FormGroup;
  passengersFormGroup!: FormGroup;

  constructor(
    private _formBuilder: FormBuilder,
    private _flightSearchSubmitService: FlightSearchSubmitService,
    private _snackBar: MatSnackBar,
    private componentFactoryResolver: ComponentFactoryResolver,
    private injector: Injector,
    private changeDetectorRef: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    this.whereFormGroup = this._formBuilder.group({});
    this.originFormGroup = this._formBuilder.group({});
    this.whenFormGroup = this._formBuilder.group({});
    this.passengersFormGroup = this._formBuilder.group({});
  }

  ngAfterViewInit() {
    setTimeout(() => {
      this.loadWeekendComponent('standard');
      this.whereFormGroup.addControl(
        'where',
        this.citiesAutocompleteWhereTo.whereToElementFormControl,
      );
      this.originFormGroup.addControl(
        'origin',
        this.originFlightSearchFormComponent.originElementFormControl,
      );
      this.passengersFormGroup.addControl(
        'slider',
        this.passengersFlightSearchFormComponent.passengerSliderFormControl,
      );
    });
  }

  onWeekendTypeChange(newType: string): void {
    console.log(`type in main: ${newType}`);
    this.loadWeekendComponent(newType);
  }

  //TODO: implement two elements to dynamically switch between instead of switching DI provider (which is not working btw,)
  loadWeekendComponent(weekendType: string): void {
    console.log('format change!!');
    this.weekendComponentContainer.clear();

    const strategyProvider = this.getStrategyProviderBasedOnType(weekendType);
    console.log(strategyProvider);
    const strategyInjector = Injector.create({
      providers: [
        {
          provide: MAT_DATE_RANGE_SELECTION_STRATEGY,
          useFactory: () => new strategyProvider(),
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
      parent: this.injector,
    });

    console.log(
      'Injected Provider: ',
      strategyInjector.get(MAT_DATE_RANGE_SELECTION_STRATEGY).constructor.name,
    );

    const componentRef =
      this.weekendComponentContainer.createComponent<WhichWeekendFlightSearchComponent>(
        WhichWeekendFlightSearchComponent,
        { injector: strategyInjector },
      );

    console.log(
      'Current provider:',
      componentRef.injector.get(MAT_DATE_RANGE_SELECTION_STRATEGY).constructor
        .name,
    );

    componentRef.instance.defaultToggleValue = weekendType;

    componentRef.instance.weekendTypeChange.subscribe((newType: string) => {
      console.log('new type yesss');
      this.onWeekendTypeChange(newType);
    });

    // Access the whichWeekendRange form group from the component instance
    const whichWeekendRange =
      componentRef.instance.whichWeekendRangeFormControl;

    // Update your form group here
    this.whenFormGroup.setControl('when', whichWeekendRange);

    this.changeDetectorRef.detectChanges();
  }

  private getStrategyProviderBasedOnType(weekendType: string): Type<any> {
    weekendType === 'long'
      ? console.log('LongWeekendRangeSelectionStategyService')
      : console.log('WeekendRangeSelectionStategyService');
    return weekendType === 'long'
      ? LongWeekendRangeSelectionStategyService
      : WeekendRangeSelectionStategyService;
    // return LongWeekendRangeSelectionStategyService;
  }

  submit() {
    if (
      this.whereFormGroup.valid &&
      this.originFormGroup.valid &&
      this.whenFormGroup.valid &&
      this.passengersFormGroup.valid
    ) {
      const serializedFlightSearchData: FlightSearchData =
        this._flightSearchSubmitService.serializeFlightSearchData(
          this.whereFormGroup.value.where,
          this.originFormGroup.value.origin,
          this.whenFormGroup.value.when.start,
          this.whenFormGroup.value.when.end,
          this.passengersFormGroup.value.slider,
        );

      this._flightSearchSubmitService
        .submitFlightSearchDataToApi(serializedFlightSearchData)
        .subscribe(
          (response) => console.log('Submit Success:', response),
          (error) => console.error('Submit Error:', error),
        );
    } else {
      console.error('Some form groups on submit are invalid.');
      this._snackBar.open(
        'Please correct the errors in the form before submitting.',
        'Close',
        {
          duration: 5000,
          panelClass: ['blue-snackbar'],
        },
      );
    }
  }
}
