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
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { MatIconModule } from '@angular/material/icon';

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

  constructor(private _formBuilder: FormBuilder) {}
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
      this.passengersFormGroup
        .get('slider')
        ?.valueChanges.subscribe((value) => {
          console.log('Slider value:', value);
        });
    });
  }
}
