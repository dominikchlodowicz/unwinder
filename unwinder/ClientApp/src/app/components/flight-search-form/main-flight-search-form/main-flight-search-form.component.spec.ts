import {
  ComponentFixture,
  TestBed,
  fakeAsync,
  tick,
} from '@angular/core/testing';
import { MainFlightSearchFormComponent } from './main-flight-search-form.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormControl,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { WhereToFlightSearchFormComponent } from '../where-to-flight-search-form/where-to-flight-search-form.component';
import { ShortWhichWeekendFlightSearchComponent } from '../short-which-weekend-flight-search-form/short-which-weekend-flight-search-form.component';
import { OriginFlightSearchFormComponent } from '../origin-flight-search-form/origin-flight-search-form.component';
import { PassengersFlightSearchFormComponent } from '../passengers-flight-search-form/passengers-flight-search-form.component';

import { MatStepperModule } from '@angular/material/stepper';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';
import { FlightSearchSubmitService } from '../../../services/flight-search-form/flight-search-submit.service';
import { of, throwError } from 'rxjs';
import { LongWhichWeekendFlightSearchFormComponent } from '../long-which-weekend-flight-search-form/long-which-weekend-flight-search-form.component';
import { BaseWhichWeekendFlightSearchFormComponent } from '../base-which-weekend-flight-search-form/base-which-weekend-flight-search-form.component';
import { ComponentRef } from '@angular/core';

describe('MainFlightSearchFormComponent', () => {
  let component: MainFlightSearchFormComponent;
  let fixture: ComponentFixture<MainFlightSearchFormComponent>;
  let mockSnackBar: MatSnackBar;
  let mockFlightSearchSubmitService: jest.Mocked<FlightSearchSubmitService>;

  beforeEach(async () => {
    // Creating mocks with Jest
    mockSnackBar = { open: jest.fn() } as unknown as MatSnackBar;
    mockFlightSearchSubmitService = {
      submitFlightSearchDataToApi: jest.fn(),
    } as unknown as jest.Mocked<FlightSearchSubmitService>;

    await TestBed.configureTestingModule({
      imports: [
        CommonModule,
        WhereToFlightSearchFormComponent,
        ShortWhichWeekendFlightSearchComponent,
        LongWhichWeekendFlightSearchFormComponent,
        // BaseWhichWeekendFlightSearchFormComponent,
        OriginFlightSearchFormComponent,
        PassengersFlightSearchFormComponent,
        MatStepperModule,
        FormsModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatIconModule,
        BrowserAnimationsModule,
      ],
      providers: [
        FormBuilder,
        { provide: MatSnackBar, useValue: mockSnackBar },
        {
          provide: FlightSearchSubmitService,
          useValue: mockFlightSearchSubmitService,
        },
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MainFlightSearchFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form groups correctly', () => {
    component.ngOnInit();
    expect(component.whereFormGroup).toBeDefined();
    expect(component.originFormGroup).toBeDefined();
    expect(component.whenFormGroup).toBeDefined();
    expect(component.passengersFormGroup).toBeDefined();
  });

  it('should reference subcomponents correctly after view init', () => {
    component.ngAfterViewInit();
    expect(component.citiesAutocompleteWhereTo).toBeDefined();
    expect(component.originFlightSearchFormComponent).toBeDefined();
    expect(component.passengersFlightSearchFormComponent).toBeDefined();
  });

  it('should add form controls to form groups after view init', fakeAsync(() => {
    component.ngOnInit();
    component.ngAfterViewInit();
    tick();
    expect(component.whereFormGroup.get('where')).toBeDefined();
    expect(component.originFormGroup.get('origin')).toBeDefined();
    expect(component.whenFormGroup.get('when')).toBeDefined();
    expect(component.passengersFormGroup.get('slider')).toBeDefined();
  }));

  it('should submit when all forms are valid', () => {
    component.ngOnInit();

    const privateComponent: any = component;

    privateComponent._flightSearchSubmitService = {
      submitFlightSearchDataToApi: jest.fn().mockReturnValue(of({})),
      serializeFlightSearchData: jest.fn(),
    };

    component.whereFormGroup.setControl('where', new FormControl('test'));
    component.originFormGroup.setControl('origin', new FormControl('test'));
    component.whenFormGroup.setControl(
      'when',
      new FormControl({ start: new Date(), end: new Date() }),
    );
    component.passengersFormGroup.setControl('slider', new FormControl(1));

    component.submit();

    expect(
      privateComponent._flightSearchSubmitService.submitFlightSearchDataToApi,
    ).toHaveBeenCalled();
  });

  it('should handle errors on submission failure', () => {
    component.ngOnInit();

    component['_flightSearchSubmitService'].serializeFlightSearchData = jest
      .fn()
      .mockImplementation(() => {
        return {};
      });

    component['_flightSearchSubmitService'].submitFlightSearchDataToApi = jest
      .fn()
      .mockReturnValue(throwError(new Error('API error')));

    component.whereFormGroup.setControl('where', new FormControl('test'));
    component.originFormGroup.setControl('origin', new FormControl('test'));
    component.whenFormGroup.setControl(
      'when',
      new FormControl({ start: new Date(), end: new Date() }),
    );
    component.passengersFormGroup.setControl('slider', new FormControl(1));

    const consoleErrorSpy = jest.spyOn(console, 'error');

    component.submit();

    expect(consoleErrorSpy).toHaveBeenCalledWith(
      'Error updating data:',
      new Error('API error'),
    );
  });

  it('should open snackbar with error message when form is invalid', async () => {
    component.ngOnInit();
    component.ngAfterViewInit();

    component.whereFormGroup.addControl(
      'where',
      new FormControl(null, Validators.required),
    );
    component.originFormGroup.addControl(
      'origin',
      new FormControl(null, Validators.required),
    );
    component.whenFormGroup.addControl(
      'when',
      new FormControl({ start: new Date(), end: new Date() }),
    );
    component.passengersFormGroup.addControl('slider', new FormControl(1));

    component['_flightSearchSubmitService'].serializeFlightSearchData = jest
      .fn()
      .mockImplementation(() => ({}));

    component['_flightSearchSubmitService'].submitFlightSearchDataToApi = jest
      .fn()
      .mockReturnValue(of({}));

    fixture.detectChanges();

    component.submit();

    expect(mockSnackBar.open).toHaveBeenCalledTimes(1);

    expect(mockSnackBar.open).toHaveBeenCalledWith(
      'Please correct the errors in the form before submitting.',
      'Close',
      expect.any(Object),
    );
  });

  it('should load ShortWhichWeekendFlightSearchComponent for "short" weekend type', () => {
    const componentRef = component.loadWeekendComponent(
      'short',
    ) as ComponentRef<ShortWhichWeekendFlightSearchComponent>;
    fixture.detectChanges();
    expect(componentRef.instance).toBeInstanceOf(
      ShortWhichWeekendFlightSearchComponent,
    );
  });

  it('should load LongWhichWeekendFlightSearchFormComponent for "long" weekend type', () => {
    const componentRef = component.loadWeekendComponent(
      'long',
    ) as ComponentRef<LongWhichWeekendFlightSearchFormComponent>;
    fixture.detectChanges();
    expect(componentRef.instance).toBeInstanceOf(
      LongWhichWeekendFlightSearchFormComponent,
    );
  });

  it('should integrate weekend component form group into whenFormGroup', () => {
    const componentRef = component.loadWeekendComponent(
      'short',
    ) as ComponentRef<ShortWhichWeekendFlightSearchComponent>;
    fixture.detectChanges();
    expect(component.whenFormGroup.get('when')).toBe(
      componentRef.instance.whichWeekendRangeFormControl,
    );
  });

  it('should handle weekendTypeChange event', () => {
    const componentRef = component.loadWeekendComponent(
      'short',
    ) as ComponentRef<ShortWhichWeekendFlightSearchComponent>;
    fixture.detectChanges();
    jest.spyOn(component, 'onWeekendTypeChange');
    componentRef.instance.weekendTypeChange.emit('long');
    expect(component.onWeekendTypeChange).toHaveBeenCalledWith('long');
  });
});
