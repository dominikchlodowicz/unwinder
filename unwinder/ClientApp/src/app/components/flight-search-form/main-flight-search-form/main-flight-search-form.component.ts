import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WhereToFlightSearchFormComponent } from '../where-to-flight-search-form/where-to-flight-search-form.component';

@Component({
  selector: 'app-main-flight-search-form',
  standalone: true,
  imports: [CommonModule, WhereToFlightSearchFormComponent],
  templateUrl: './main-flight-search-form.component.html',
  styleUrl: './main-flight-search-form.component.css',
})
export class MainFlightSearchFormComponent {}
