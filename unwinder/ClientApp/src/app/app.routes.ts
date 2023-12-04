import { Routes } from '@angular/router';
import { MainFlightSearchFormComponent } from './components/flight-search-form/main-flight-search-form/main-flight-search-form.component';
import { WhereToFlightSearchFormComponent } from './components/flight-search-form/where-to-flight-search-form/where-to-flight-search-form.component';

export const routes: Routes = [
  { path: 'flight-search', component: MainFlightSearchFormComponent },
  { path: 'where-to', component: WhereToFlightSearchFormComponent },
];
