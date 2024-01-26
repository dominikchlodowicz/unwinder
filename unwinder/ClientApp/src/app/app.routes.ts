import { Routes } from '@angular/router';
import { MainFlightSearchFormComponent } from './components/flight-search-form/main-flight-search-form/main-flight-search-form.component';
import { FlightOfferCardComponent } from './components/flight-search-results/flight-offer-card/flight-offer-card.component';

export const routes: Routes = [
  { path: 'unwind/flight-search', component: MainFlightSearchFormComponent },
  {
    path: 'unwind/flight-search/first-flight',
    component: FlightOfferCardComponent,
  },
  // { path: 'unwind/flight-search/second-flight', component:  },
  // { path: 'unwind/hotel', component:  },
];
