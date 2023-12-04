import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { OnInit } from '@angular/core';

@Component({
  selector: 'app-where-to-flight-search-form',
  standalone: true,
  imports: [
    CommonModule,
    MatAutocompleteModule,
    ReactiveFormsModule,
    MatInputModule,
  ],
  templateUrl: './where-to-flight-search-form.component.html',
  styleUrl: './where-to-flight-search-form.component.css',
})
export class WhereToFlightSearchFormComponent implements OnInit {
  cities = ['test1', 'test2', 'test3'];
  filteredCities: string[] = [];
  citiesAutocomplete = new FormControl();

  ngOnInit(): void {
    this.citiesAutocomplete.valueChanges.subscribe((newValue) => {
      this.filteredCities = this.filterValues(newValue);
    });
  }

  filterValues(search: string): string[] {
    return this.cities.filter(
      (value) => value.toLowerCase().indexOf(search.toLowerCase()) === 0,
    );
  }
}
