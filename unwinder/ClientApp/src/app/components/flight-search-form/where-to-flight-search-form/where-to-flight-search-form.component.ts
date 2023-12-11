import { Component, inject, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { OnInit } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FlightSearchCitiesService } from '../../../services/flight-search-form/flight-search-cities.service';
import { switchMap, debounceTime } from 'rxjs';

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
  encapsulation: ViewEncapsulation.None
})
export class WhereToFlightSearchFormComponent implements OnInit {
  responseCities: string[] = [];

  constructor(private flightSearchCitiesService: FlightSearchCitiesService) {}

  filteredCities: string[] = [];
  citiesAutocomplete = new FormControl();

  ngOnInit(): void {
    this.citiesAutocomplete.valueChanges
      .pipe(
        debounceTime(300),
        switchMap((newValue) =>
          this.flightSearchCitiesService.getCities(newValue),
        ),
      )
      .subscribe((cities) => {
        this.responseCities = cities;
        this.filteredCities = this.filterValues(this.citiesAutocomplete.value);
      });
  }

  filterValues(search: string): string[] {
    return this.responseCities.filter(
      (value) =>
        String(value).toLowerCase().indexOf(search.toLowerCase()) === 0,
    );
  }
}
