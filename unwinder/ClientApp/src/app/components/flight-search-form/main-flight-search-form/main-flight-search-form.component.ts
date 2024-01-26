import {
  ChangeDetectorRef,
  Component,
  ComponentRef,
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
import { ShortWhichWeekendFlightSearchComponent } from '../short-which-weekend-flight-search-form/short-which-weekend-flight-search-form.component';
import { LongWhichWeekendFlightSearchFormComponent } from '../long-which-weekend-flight-search-form/long-which-weekend-flight-search-form.component';
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
import { BaseWhichWeekendFlightSearchFormComponent } from '../base-which-weekend-flight-search-form/base-which-weekend-flight-search-form.component';
import { UnwinderSessionService } from '../../../services/unwinder-search-state/unwinder-session.service';

@Component({
  selector: 'app-main-flight-search-form',
  standalone: true,
  imports: [
    CommonModule,
    WhereToFlightSearchFormComponent,
    ShortWhichWeekendFlightSearchComponent,
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
  weekendComponentContainer!: ViewContainerRef;

  whereFormGroup!: FormGroup;
  originFormGroup!: FormGroup;
  whenFormGroup!: FormGroup;
  passengersFormGroup!: FormGroup;

  constructor(
    private _formBuilder: FormBuilder,
    private _flightSearchSubmitService: FlightSearchSubmitService,
    private _snackBar: MatSnackBar,
    private _changeDetectorRef: ChangeDetectorRef,
    private _flightSearchSessionService: UnwinderSessionService,
  ) {}

  ngOnInit() {
    this.whereFormGroup = this._formBuilder.group({});
    this.originFormGroup = this._formBuilder.group({});
    this.whenFormGroup = this._formBuilder.group({});
    this.passengersFormGroup = this._formBuilder.group({});
  }

  ngAfterViewInit() {
    setTimeout(() => {
      this.loadWeekendComponent('short');
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
    this.loadWeekendComponent(newType);
  }

  loadWeekendComponent(
    weekendType: string,
  ): ComponentRef<BaseWhichWeekendFlightSearchFormComponent> {
    this.weekendComponentContainer.clear();

    const componentToLoad: Type<BaseWhichWeekendFlightSearchFormComponent> =
      weekendType === 'long'
        ? LongWhichWeekendFlightSearchFormComponent
        : ShortWhichWeekendFlightSearchComponent;

    const componentRef =
      this.weekendComponentContainer.createComponent(componentToLoad);

    componentRef.instance.defaultToggleValue = weekendType;

    componentRef.instance.weekendTypeChange.subscribe((newType: string) => {
      this.onWeekendTypeChange(newType);
    });

    // Access the whichWeekendRange form group from the component instance
    const whichWeekendRange =
      componentRef.instance.whichWeekendRangeFormControl;

    // Update your form group here
    this.whenFormGroup.setControl('when', whichWeekendRange);

    this._changeDetectorRef.detectChanges();

    return componentRef;
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
          this.passengersFormGroup.value.slider,
        );

      this._flightSearchSessionService.setData(
        'flightBackData',
        this._flightSearchSubmitService.serializeFlightBackData(
          this.whenFormGroup.value.when.end,
        ),
      );

      this._flightSearchSubmitService
        .submitFlightSearchDataToApi(serializedFlightSearchData)
        .subscribe({
          next: (response) => {
            this._flightSearchSessionService.setData(
              'chosenFirstFlightData',
              response,
            );

            console.log('Submit Success:', response);
          },
          error: (error) => console.error('Submit Error:', error),
        });
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
