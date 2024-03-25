import { Routes } from '@angular/router';
import { MainFlightSearchFormComponent } from './components/flight-search-form/main-flight-search-form/main-flight-search-form.component';
import { FirstFlightOfferMainComponent } from './components/flight-search-results/first-flight-offer-main/first-flight-offer-main.component';
import { SecondFlightOfferMainComponent } from './components/flight-search-results/second-flight-offer-main/second-flight-offer-main.component';
import { AppComponent } from './app.component';
import { MainHomepageContentComponent } from './components/homepage/main-homepage-content/main-homepage-content.component';
import { HotelCardComponent } from './components/hotel-search-results/hotel-card/hotel-card.component';

export const routes: Routes = [
  { path: '', component: MainHomepageContentComponent },
  { path: 'unwind/flight-search', component: MainFlightSearchFormComponent },
  {
    path: 'unwind/first-flight',
    component: FirstFlightOfferMainComponent,
  },
  { path: 'unwind/second-flight', component: SecondFlightOfferMainComponent },
  { path: 'test/hotelcard', component: HotelCardComponent },
];
