import { Routes } from '@angular/router';
import { MainFlightSearchFormComponent } from './components/flight-search-form/main-flight-search-form/main-flight-search-form.component';
import { FirstFlightOfferMainComponent } from './components/flight-search-results/first-flight-offer-main/first-flight-offer-main.component';
import { SecondFlightOfferMainComponent } from './components/flight-search-results/second-flight-offer-main/second-flight-offer-main.component';
import { AppComponent } from './app.component';

export const routes: Routes = [
  { path: './', component: AppComponent },
  { path: 'unwind/flight-search', component: MainFlightSearchFormComponent },
  {
    path: 'unwind/first-flight',
    component: FirstFlightOfferMainComponent,
  },
  { path: 'unwind/second-flight', component: SecondFlightOfferMainComponent },
];
