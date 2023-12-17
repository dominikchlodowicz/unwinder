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

import { MatStepperModule } from '@angular/material/stepper';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-main-flight-search-form',
  standalone: true,
  imports: [
    CommonModule,
    WhereToFlightSearchFormComponent,
    WhichWeekendFlightSearchComponent,
    OriginFlightSearchFormComponent,
    MatStepperModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
  ],
  templateUrl: './main-flight-search-form.component.html',
  styleUrl: './main-flight-search-form.component.css',
})
export class MainFlightSearchFormComponent implements OnInit {
  @ViewChild(WhereToFlightSearchFormComponent)
  citiesAutocompleteWhereTo!: WhereToFlightSearchFormComponent;

  @ViewChild(OriginFlightSearchFormComponent)
  originFlightSearchFormComponent!: OriginFlightSearchFormComponent;

  @ViewChild(WhichWeekendFlightSearchComponent)
  wichWeekendFlightSearchComponent!: WhichWeekendFlightSearchComponent;

  whereFormGroup!: FormGroup;
  originFormGroup!: FormGroup;
  whenFormGroup!: FormGroup;

  constructor(private _formBuilder: FormBuilder) {}
  ngOnInit() {
    // Initialize empty FormGroups which will be linked to child controls
    this.whereFormGroup = this._formBuilder.group({});
    this.originFormGroup = this._formBuilder.group({});
    this.whenFormGroup = this._formBuilder.group({});
  }

  ngAfterViewInit() {
    // Link child component controls to the FormGroup
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
  }
}
