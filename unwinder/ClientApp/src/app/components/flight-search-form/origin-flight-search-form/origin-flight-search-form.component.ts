import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OnInit } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule, FormControl, Validators } from '@angular/forms';

import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';

import { FlightSearchCitiesService } from '../../../services/flight-search-form/flight-search-cities.service';
import { switchMap, debounceTime, filter, tap } from 'rxjs';

@Component({
  selector: 'app-origin-flight-search-form',
  standalone: true,
  imports: [
    CommonModule,
    MatAutocompleteModule,
    ReactiveFormsModule,
    MatInputModule,
    HttpClientModule,
  ],
  templateUrl: './origin-flight-search-form.component.html',
  styleUrl: './origin-flight-search-form.component.scss',
})
export class OriginFlightSearchFormComponent implements OnInit {
  autocompleteOptionSelected = false;
  responseCities: string[] = [];
  private selectedFromDropdown = false;

  constructor(private flightSearchCitiesService: FlightSearchCitiesService) {}

  filteredCities: string[] = [];
  citiesAutocompleteOrigin = new FormControl('', [
    Validators.required,
    Validators.minLength(3),
  ]);

  ngOnInit(): void {
    this.citiesAutocompleteOrigin.valueChanges
      .pipe(
        debounceTime(300),
        filter(
          (newValue) =>
            typeof newValue === 'string' && !this.selectedFromDropdown,
        ),
        switchMap((newValue) =>
          this.flightSearchCitiesService.getCities(newValue!),
        ),
      )
      .subscribe((cities) => {
        this.responseCities = cities;
        this.filteredCities = this.filterValues(
          this.citiesAutocompleteOrigin.value!,
        );
        this.selectedFromDropdown = false;
      });
  }

  get originElementFormControl(): FormControl {
    return this.citiesAutocompleteOrigin;
  }

  filterValues(search: string): string[] {
    return this.responseCities.filter(
      (value) =>
        String(value).toLowerCase().indexOf(search.toLowerCase()) === 0,
    );
  }

  onOptionSelected() {
    this.selectedFromDropdown = true;
  }
}
