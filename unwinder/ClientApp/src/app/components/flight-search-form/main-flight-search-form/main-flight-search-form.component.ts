import { Component, OnInit, ViewChild } from '@angular/core';
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

  @ViewChild(WhichWeekendFlightSearchComponent)
  public wichWeekendFlightSearchComponent!: WhichWeekendFlightSearchComponent;

  @ViewChild(PassengersFlightSearchFormComponent)
  public passengersFlightSearchFormComponent!: PassengersFlightSearchFormComponent;

  whereFormGroup!: FormGroup;
  originFormGroup!: FormGroup;
  whenFormGroup!: FormGroup;
  passengersFormGroup!: FormGroup;

  constructor(
    private _formBuilder: FormBuilder,
    private _flightSearchSubmitService: FlightSearchSubmitService,
    private _snackBar: MatSnackBar,
  ) {}

  ngOnInit() {
    this.whereFormGroup = this._formBuilder.group({});
    this.originFormGroup = this._formBuilder.group({});
    this.whenFormGroup = this._formBuilder.group({});
    this.passengersFormGroup = this._formBuilder.group({});
  }

  ngAfterViewInit() {
    setTimeout(() => {
      this.whereFormGroup.addControl(
        'where',
        this.citiesAutocompleteWhereTo.whereToElementFormControl,
      );
      this.originFormGroup.addControl(
        'origin',
        this.originFlightSearchFormComponent.originElementFormControl,
      );
      this.whenFormGroup.addControl(
        'when',
        this.wichWeekendFlightSearchComponent.whichWeekendRangeFormControl,
      );
      this.passengersFormGroup.addControl(
        'slider',
        this.passengersFlightSearchFormComponent.passengerSliderFormControl,
      );
    });
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
