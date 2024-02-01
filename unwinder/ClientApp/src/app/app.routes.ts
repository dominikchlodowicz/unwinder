import { Routes } from '@angular/router';
import { MainFlightSearchFormComponent } from './components/flight-search-form/main-flight-search-form/main-flight-search-form.component';
import { FlightOfferMainComponent } from './components/flight-search-results/flight-offer-main/flight-offer-main.component';
import { FlightOfferCardComponent } from './components/flight-search-results/flight-offer-card/flight-offer-card.component';

export const routes: Routes = [
  { path: 'unwind/flight-search', component: MainFlightSearchFormComponent },
  {
    path: 'unwind/first-flight',
    component: FlightOfferMainComponent,
  },
  { path: 'unwind/second-flight', component: FlightOfferMainComponent },
  // Path for dev purposes
  {
    path: 'dev/card',
    component: FlightOfferCardComponent,
  },
  // { path: 'unwind/hotel', component:  },
];
