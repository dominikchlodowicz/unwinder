import { Routes } from '@angular/router';
import { MainFlightSearchFormComponent } from './components/flight-search-form/main-flight-search-form/main-flight-search-form.component';
import { FirstFlightOfferMainComponent } from './components/flight-search-results/first-flight-offer-main/first-flight-offer-main.component';
import { SecondFlightOfferMainComponent } from './components/flight-search-results/second-flight-offer-main/second-flight-offer-main.component';
import { MainHomepageContentComponent } from './components/homepage/main-homepage-content/main-homepage-content.component';
import { HotelSearchResultsComponent } from './components/hotel-search-results/hotel-search-results/hotel-search-results.component';
import { FlightNotFoundComponent } from './components/not-found/flight-not-found/flight-not-found.component';
import { HotelNotFoundComponent } from './components/not-found/hotel-not-found/hotel-not-found.component';

export const routes: Routes = [
  { path: '', component: MainHomepageContentComponent },
  { path: 'unwind/flight-search', component: MainFlightSearchFormComponent },
  {
    path: 'unwind/first-flight',
    component: FirstFlightOfferMainComponent,
  },
  { path: 'unwind/second-flight', component: SecondFlightOfferMainComponent },
  { path: 'unwind/hotel', component: HotelSearchResultsComponent },
  { path: 'unwind/flighterror', component: FlightNotFoundComponent },
  { path: 'unwind/hotelerror', component: HotelNotFoundComponent },
];
