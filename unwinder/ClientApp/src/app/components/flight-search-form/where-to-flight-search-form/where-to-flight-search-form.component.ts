import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';
import { ReactiveFormsModule, FormControl, Validators } from '@angular/forms';
import { OnInit } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FlightSearchCitiesService } from '../../../services/flight-search-form/flight-search-cities.service';
import { switchMap, debounceTime, filter, tap } from 'rxjs';

@Component({
  selector: 'app-where-to-flight-search-form',
  standalone: true,
  imports: [
    CommonModule,
    MatAutocompleteModule,
    ReactiveFormsModule,
    MatInputModule,
    HttpClientModule,
  ],
  templateUrl: './where-to-flight-search-form.component.html',
  styleUrl: './where-to-flight-search-form.component.css',
})
export class WhereToFlightSearchFormComponent implements OnInit {
  autocompleteOptionSelected = false;
  responseCities: string[] = [];
  private selectedFromDropdown = false;

  constructor(private flightSearchCitiesService: FlightSearchCitiesService) {}

  filteredCities: string[] = [];
  citiesAutocompleteWhereTo = new FormControl('', [Validators.required, Validators.minLength(3)]);

  ngOnInit(): void {
    this.citiesAutocompleteWhereTo.valueChanges
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
          this.citiesAutocompleteWhereTo.value!,
        );
        this.selectedFromDropdown = false;
      });
  }

  get whereToElementFormControl(): FormControl {
    return this.citiesAutocompleteWhereTo;
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
